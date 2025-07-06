// src/BasketStats.Application/Competitions/Commands/CreateCompetition/CreateCompetitionCommand.cs
using MediatR;

namespace BasketStats.Application.Competitions.Commands.CreateCompetition;

public record CreateCompetitionCommand(string Name) : IRequest<Guid>;