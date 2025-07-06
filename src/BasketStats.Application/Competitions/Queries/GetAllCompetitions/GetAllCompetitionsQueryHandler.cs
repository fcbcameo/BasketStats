// src/BasketStats.Application/Competitions/Queries/GetAllCompetitions/GetAllCompetitionsQueryHandler.cs
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Competitions.Queries.GetAllCompetitions;

public class GetAllCompetitionsQueryHandler : IRequestHandler<GetAllCompetitionsQuery, IEnumerable<CompetitionDto>>
{
    private readonly ICompetitionRepository _competitionRepository;

    public GetAllCompetitionsQueryHandler(ICompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    public async Task<IEnumerable<CompetitionDto>> Handle(GetAllCompetitionsQuery request, CancellationToken cancellationToken)
    {
        var competitions = await _competitionRepository.GetAllAsync();

        return competitions.Select(c => new CompetitionDto
        {
            Id = c.Id,
            Name = c.Name
        });
    }
}