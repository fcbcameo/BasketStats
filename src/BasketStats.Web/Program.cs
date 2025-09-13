// src/BasketStats.Web/Program.cs
using BasketStats.Application.Common.Behaviors;
using BasketStats.Application.Competitions.Commands.CreateCompetition;
using BasketStats.Application.Competitions.Commands.DeleteCompetition;
using BasketStats.Application.Competitions.Queries.GetAllCompetitions;
using BasketStats.Application.DTOs;
using BasketStats.Application.Matches.Commands.UploadMatchStats;
using BasketStats.Application.Matches.Commands.DeleteMatch;
using BasketStats.Application.Players.Queries.GetPlayerSeasonStats;
using BasketStats.Application.Players.Queries.GetAllPlayersStats;
using BasketStats.Application.Services;
using BasketStats.Application.Teams.Queries.GetTeamSeasonStats;
using BasketStats.Domain.Repositories;
using BasketStats.Infrastructure.Persistence;
using BasketStats.Infrastructure.Persistence.Repositories;
using BasketStats.Infrastructure.Services;
using BasketStats.Web.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddSingleton<IMatchRepository, InMemoryMatchRepository>();
//builder.Services.AddTransient<ICsvParser, CsvParserService>(); // Transient is fine for a stateless service

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketStats API", Version = "v1" });
});

// CORS for Blazor and Angular dev hosts
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.WithOrigins(
                "https://localhost:7275", // Blazor
                "http://localhost:5196",
                "https://localhost:7200", // Angular ASP.NET Core host
                "http://localhost:4200",   // Angular CLI dev server
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Register our repository for Dependency Injection
//builder.Services.AddSingleton<ICompetitionRepository, InMemoryCompetitionRepository>();

// Register MediatR and handlers from the Application assembly
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCompetitionCommand).Assembly));

builder.Services.AddTransient<ICsvParser, CsvParserService>();

builder.Services.AddScoped<ICompetitionRepository, EfCompetitionRepository>();
builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>(); 
builder.Services.AddScoped<IMatchRepository, EfMatchRepository>();

// Register MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCompetitionCommand).Assembly));

// *** ADD VALIDATION BEHAVIOR ***
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// *** ADD VALIDATORS ***
builder.Services.AddValidatorsFromAssembly(typeof(CreateCompetitionCommand).Assembly);


// Add Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/basketstats-api.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("DevCors");
app.UseRouting();

// NOTE: antiforgery middleware removed intentionally because this app runs locally and
// Swagger does not send antiforgery tokens. We avoid declaring IFormFile parameters on
// minimal API delegates (which would add antiforgery metadata). Instead read uploaded
// files from the HttpRequest form.

// Helper to get IFormFile from request, with fallback to raw body
static async Task<IFormFile?> GetUploadedFileAsync(HttpRequest request)
{
    // If it's a form, prefer form files
    if (request.HasFormContentType)
    {
        try
        {
            var form = await request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();
            if (file != null) return file;
        }
        catch
        {
            // ignore and fall through to other handling
        }
    }

    // If content type indicates multipart but ReadFormAsync didn't work, try manual parsing using MultipartReader
    if (!string.IsNullOrEmpty(request.ContentType) && request.ContentType.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase))
    {
        // Extract boundary
        var boundary = GetBoundary(request.ContentType);
        if (!string.IsNullOrEmpty(boundary))
        {
            try
            {
                var reader = new MultipartReader(boundary, request.Body);
                MultipartSection? section;
                while ((section = await reader.ReadNextSectionAsync()) != null)
                {
                    var contentDisposition = section.ContentDisposition;
                    if (string.IsNullOrEmpty(contentDisposition)) continue;

                    var contentDisp = ContentDispositionHeaderValue.Parse(contentDisposition);
                    var dispType = contentDisp.DispositionType.HasValue ? contentDisp.DispositionType.Value : contentDisp.DispositionType.ToString();
                    if (string.Equals(dispType, "form-data", StringComparison.OrdinalIgnoreCase) &&
                        (!string.IsNullOrEmpty(contentDisp.FileName.Value) || !string.IsNullOrEmpty(contentDisp.FileNameStar.Value)))
                    {
                        var fileName = !string.IsNullOrEmpty(contentDisp.FileName.Value) ? contentDisp.FileName.Value.Trim('"') : (!string.IsNullOrEmpty(contentDisp.FileNameStar.Value) ? contentDisp.FileNameStar.Value : "upload.bin");

                        var ms = new MemoryStream();
                        await section.Body.CopyToAsync(ms);
                        ms.Position = 0;

                        var name = !string.IsNullOrEmpty(contentDisp.Name.Value) ? contentDisp.Name.Value : "file";

                        var formFile = new FormFile(ms, 0, ms.Length, name, fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = section.ContentType ?? request.ContentType
                        };

                        return formFile;
                    }
                }
            }
            catch
            {
                // ignore and fall through to raw body handling
            }
        }
    }

    // Fallback: read the raw request body regardless of Content-Length (handles chunked/unknown length)
    try
    {
        // If the body stream is null, return
        if (request.Body == null)
            return null;

        // Copy the body to a MemoryStream
        var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms);

        if (ms.Length == 0)
            return null;

        ms.Position = 0;
        var fileName = "upload.bin";

        // Try to obtain a filename from Content-Disposition if present
        if (request.Headers.TryGetValue("Content-Disposition", out StringValues cd))
        {
            var cdValue = cd.ToString();
            var match = System.Text.RegularExpressions.Regex.Match(cdValue, "filename=\"?(?<name>[^\";]+)\"?");
            if (match.Success)
            {
                fileName = match.Groups["name"].Value;
            }
        }

        var formFile = new FormFile(ms, 0, ms.Length, "file", fileName)
        {
            Headers = request.Headers,
            ContentType = request.ContentType ?? "application/octet-stream"
        };

        return formFile;
    }
    catch
    {
        return null;
    }
}

static string? GetBoundary(string contentType)
{
    // contentType is expected like: multipart/form-data; boundary=----WebKitFormBoundaryxyz
    var elements = contentType.Split(';');
    foreach (var element in elements)
    {
        var trimmed = element.Trim();
        if (trimmed.StartsWith("boundary=", StringComparison.OrdinalIgnoreCase))
        {
            var boundary = trimmed.Substring("boundary=".Length);
            // Remove quotes if present
            if (boundary.Length >= 2 && boundary[0] == '"' && boundary[boundary.Length - 1] == '"')
            {
                boundary = boundary.Substring(1, boundary.Length - 2);
            }
            // MultipartReader expects boundary without the leading --
            if (boundary.StartsWith("--"))
                boundary = boundary.Substring(2);
            return boundary;
        }
    }
    return null;
}

// Refactored API Endpoints

app.MapPost("/api/competitions/{competitionId}/matches",
    async (Guid competitionId, HttpRequest request, IMediator mediator, ILogger<Program> logger) =>
    {
        var file = await GetUploadedFileAsync(request);

        logger.LogInformation("Upload endpoint called for competition {CompetitionId} with file {FileName}, length {FileLength}, content type {ContentType}", competitionId, file?.FileName, file?.Length, file?.ContentType);
        try
        {
            if (file == null)
            {
                logger.LogWarning("No file provided in upload request for competition {CompetitionId}", competitionId);
                return Results.BadRequest("No file provided.");
            }
            if (file.Length == 0)
            {
                logger.LogWarning("Empty file uploaded for competition {CompetitionId}", competitionId);
                return Results.BadRequest("File is empty.");
            }

            var command = new UploadMatchStatsCommand(competitionId, file);
            logger.LogInformation("Dispatching UploadMatchStatsCommand for competition {CompetitionId}", competitionId);
            var matchId = await mediator.Send(command);
            logger.LogInformation("UploadMatchStatsCommand completed for competition {CompetitionId}, matchId {MatchId}", competitionId, matchId);

            return Results.Created($"/api/matches/{matchId}", new { matchId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading match stats for competition {CompetitionId}. Exception type: {ExceptionType}. Message: {ExceptionMessage}", competitionId, ex.GetType().FullName, ex.Message);
            if (ex.InnerException != null)
            {
                logger.LogError("Inner exception: {InnerExceptionType} - {InnerExceptionMessage}", ex.InnerException.GetType().FullName, ex.InnerException.Message);
            }
            return Results.Problem($"Exception: {ex.GetType().FullName}: {ex.Message} {(ex.InnerException != null ? "Inner: " + ex.InnerException.GetType().FullName + ": " + ex.InnerException.Message : "")}");
        }
    })
.Accepts<IFormFile>("multipart/form-data")
.Produces(201)
.Produces(400)
.Produces(500);

app.MapPost("/api/debug-upload", async (HttpRequest request, ILogger<Program> logger) =>
{
    var file = await GetUploadedFileAsync(request);

    logger.LogInformation("Debug upload called. File: {FileName}, Length: {FileLength}, ContentType: {ContentType}", file?.FileName, file?.Length, file?.ContentType);
    if (file == null) return Results.BadRequest("No file provided.");
    if (file.Length == 0) return Results.BadRequest("File is empty.");
    return Results.Ok("File received.");
}).Accepts<IFormFile>("multipart/form-data");

app.MapGet("/api/competitions", async (IMediator mediator) =>
{
    var competitions = await mediator.Send(new GetAllCompetitionsQuery());
    return Results.Ok(competitions);
});

app.MapPost("/api/competitions", async (
    [FromBody] CreateCompetitionRequest request,
    IMediator mediator,
    ILogger<Program> logger) => // Inject the logger
{
    // --- DEBUGGING STEP ---
    // Log the request object as soon as it arrives.
    logger.LogInformation("API received request to create competition with name: '{CompetitionName}'", request.Name);

    var command = new CreateCompetitionCommand(request.Name);
    var competitionId = await mediator.Send(command);
    return Results.Created($"/api/competitions/{competitionId}", new { Id = competitionId, Name = request.Name });
});

app.MapGet("/api/players", async (IPlayerRepository repo) =>
{
    // We need to cast to the concrete type to access the helper method.
    // This is okay for a temporary, in-memory testing scenario.
    if (repo is EfPlayerRepository playerRepo)
    {
        var players = await playerRepo.GetAllAsync();

        if (players is null || !players.Any())
        {
            return Results.NotFound("No players found.");
        }

        // Ensure you are returning the shared DTO
        var playerDtos = players.Select(p => new PlayerDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();

        return Results.Ok(playerDtos);
    }
    return Results.NotFound();
});

app.MapGet("/api/players/{playerId}/stats",
    async (Guid playerId, [FromQuery] Guid? competitionId, IMediator mediator) =>
    {
        var query = new GetPlayerSeasonStatsQuery(playerId, competitionId);
        var result = await mediator.Send(query);

        return result is not null ? Results.Ok(result) : Results.NotFound();
    });

app.MapGet("/api/players/stats",
    async ([FromQuery] Guid? competitionId, IMediator mediator) =>
    {
        var query = new GetAllPlayersStatsQuery(competitionId);
        var result = await mediator.Send(query);
        return Results.Ok(result);
    });

app.MapGet("/api/team/stats",
    async ([FromQuery] Guid? competitionId, IMediator mediator) =>
    {
        var query = new GetTeamSeasonStatsQuery(competitionId);
        var result = await mediator.Send(query);
        return Results.Ok(result);
    });

app.MapDelete("/api/matches/{matchId:guid}", async (Guid matchId, IMediator mediator) =>
{
    await mediator.Send(new DeleteMatchCommand(matchId));
    return Results.NoContent(); // 204 No Content is the standard response for a successful delete
})
.WithName("DeleteMatch")
.Produces(204)
.Produces(404);

app.MapDelete("/api/competitions/{competitionId:guid}", async (Guid competitionId, IMediator mediator) =>
{
    await mediator.Send(new DeleteCompetitionCommand(competitionId));
    return Results.NoContent();
})
.WithName("DeleteCompetition")
.Produces(204);

app.MapGet("/api/matches", async (IMatchRepository repo) =>
{
    var matches = await repo.GetAllAsync();
    var dtos = matches.Select(m => new MatchDto
    {
        Id = m.Id,
        MatchDate = m.MatchDate,
        CompetitionId = m.CompetitionId,
        PlayerStatCount = m.PlayerStats.Count
    });
    return Results.Ok(dtos);
});

app.Run();

// DTO for the request
public record CreateCompetitionRequest(string Name);