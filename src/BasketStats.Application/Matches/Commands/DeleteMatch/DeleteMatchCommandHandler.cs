// in src/BasketStats.Application/Matches/Commands/DeleteMatch/DeleteMatchCommandHandler.cs
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Matches.Commands.DeleteMatch;

public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand>
{
    private readonly IMatchRepository _matchRepository;

    public DeleteMatchCommandHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetByIdAsync(request.MatchId);

        if (match is not null)
        {
            await _matchRepository.DeleteAsync(match);
        }
        // If match is null, it's already gone. We don't need to do anything.
    }
}