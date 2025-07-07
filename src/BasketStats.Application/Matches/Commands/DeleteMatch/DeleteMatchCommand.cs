// in src/BasketStats.Application/Matches/Commands/DeleteMatch/DeleteMatchCommand.cs
using MediatR;

namespace BasketStats.Application.Matches.Commands.DeleteMatch;

public record DeleteMatchCommand(Guid MatchId) : IRequest;