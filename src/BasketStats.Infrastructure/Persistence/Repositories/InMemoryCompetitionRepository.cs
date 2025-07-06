// src/BasketStats.Infrastructure/Persistence/Repositories/InMemoryCompetitionRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class InMemoryCompetitionRepository : ICompetitionRepository
{
    private static readonly List<Competition> _competitions = new();

    public Task AddAsync(Competition competition)
    {
        _competitions.Add(competition);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Competition>> GetAllAsync()
    {
        return Task.FromResult(_competitions.AsEnumerable());
    }

    public Task<Competition?> GetByIdAsync(Guid id)
    {
        var competition = _competitions.SingleOrDefault(c => c.Id == id);
        return Task.FromResult(competition);
    }
}