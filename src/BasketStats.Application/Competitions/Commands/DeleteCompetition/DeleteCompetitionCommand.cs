using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Competitions.Commands.DeleteCompetition;

public record DeleteCompetitionCommand(Guid CompetitionId) : IRequest;

public class DeleteCompetitionCommandHandler : IRequestHandler<DeleteCompetitionCommand>
{
    private readonly ICompetitionRepository _repo;

    public DeleteCompetitionCommandHandler(ICompetitionRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteCompetitionCommand request, CancellationToken cancellationToken)
    {
        var comp = await _repo.GetByIdAsync(request.CompetitionId);
        if (comp is null) return; // idempotent
        await _repo.DeleteAsync(comp);
    }
}
