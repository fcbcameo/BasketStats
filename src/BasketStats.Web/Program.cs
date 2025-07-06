// src/BasketStats.Web/Program.cs
using BasketStats.Application.Competitions.Commands.CreateCompetition;
using BasketStats.Application.Competitions.Queries.GetAllCompetitions;
using BasketStats.Domain.Repositories;
using BasketStats.Infrastructure.Persistence.Repositories;
using BasketStats.Application.Services;
using BasketStats.Infrastructure.Services;
using BasketStats.Application.Matches.Commands.UploadMatchStats;
using BasketStats.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMatchRepository, InMemoryMatchRepository>();
builder.Services.AddTransient<ICsvParser, CsvParserService>(); // Transient is fine for a stateless service

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketStats API", Version = "v1" });
});

// Register our repository for Dependency Injection
builder.Services.AddSingleton<ICompetitionRepository, InMemoryCompetitionRepository>();

// Register MediatR and handlers from the Application assembly
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCompetitionCommand).Assembly));

//builder.Services.AddAntiforgery();

builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
builder.Services.AddSingleton<IMatchRepository, InMemoryMatchRepository>();
builder.Services.AddTransient<ICsvParser, CsvParserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// If you add authentication/authorization middleware, place them here:
// app.UseAuthentication();
// app.UseAuthorization();

// If you use routing/endpoints, add them like this:
// app.UseRouting();
// app.UseAntiforgery();
// app.UseEndpoints(endpoints => { /* map endpoints here */ });

// For minimal APIs (as in your code), place app.UseAntiforgery() after HTTPS redirection:
//app.UseAntiforgery();

// Refactored API Endpoints

app.MapPost("/api/competitions/{competitionId}/matches",
    async (Guid competitionId, IFormFile file, IMediator mediator) =>
    {
        if (file.Length == 0)
        {
            return Results.BadRequest("File is empty.");
        }

        var command = new UploadMatchStatsCommand(competitionId, file);
        var matchId = await mediator.Send(command);

        return Results.Created($"/api/matches/{matchId}", new { matchId });
    })
.Accepts<IFormFile>("multipart/form-data") // Hint for Swagger
.Produces(201)
.Produces(400)
.DisableAntiforgery();

app.MapGet("/api/competitions", async (IMediator mediator) =>
{
    var competitions = await mediator.Send(new GetAllCompetitionsQuery());
    return Results.Ok(competitions);
});

app.MapPost("/api/competitions", async ([FromBody] CreateCompetitionRequest request, IMediator mediator) =>
{
    try
    {
        var command = new CreateCompetitionCommand(request.Name);
        var competitionId = await mediator.Send(command);
        return Results.Created($"/api/competitions/{competitionId}", new { Id = competitionId, request.Name });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/players", async (IPlayerRepository repo) =>
{
    // We need to cast to the concrete type to access the helper method.
    // This is okay for a temporary, in-memory testing scenario.
    if (repo is InMemoryPlayerRepository playerRepo)
    {
        var players = await playerRepo.GetAllAsync();
        return Results.Ok(players);
    }
    return Results.NotFound();
});

app.Run();

// DTO for the request
public record CreateCompetitionRequest(string Name);