// src/BasketStats.Web/Program.cs
using BasketStats.Application.Competitions.Commands.CreateCompetition;
using BasketStats.Application.Competitions.Queries.GetAllCompetitions;
using BasketStats.Domain.Repositories;
using BasketStats.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Refactored API Endpoints
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

app.Run();

// DTO for the request
public record CreateCompetitionRequest(string Name);