// src/BasketStats.Application/Competitions/Commands/CreateCompetition/CreateCompetitionCommandHandler.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Competitions.Commands.CreateCompetition;

public class CreateCompetitionCommandHandler : IRequestHandler<CreateCompetitionCommand, Guid>
{
    private readonly ICompetitionRepository _competitionRepository;

    public CreateCompetitionCommandHandler(ICompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    public async Task<Guid> Handle(CreateCompetitionCommand request, CancellationToken cancellationToken)
    {
        // 1. Create the domain entity
        var competition = Competition.Create(request.Name);

        // 2. Add it to the repository
        await _competitionRepository.AddAsync(competition);

        // 3. Return the new ID
        return competition.Id;
    }
}