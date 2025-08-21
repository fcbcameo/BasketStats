// src/BasketStats.Infrastructure/Persistence/Repositories/InMemoryCompetitionRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class InMemoryCompetitionRepository : ICompetitionRepository
{
    private readonly List<Competition> _comps = new();

    public Task AddAsync(Competition competition)
    {
        _comps.Add(competition);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Competition>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Competition>>(_comps.ToList());
    }

    public Task<Competition?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_comps.FirstOrDefault(c => c.Id == id));
    }

    public Task DeleteAsync(Competition competition)
    {
        _comps.RemoveAll(c => c.Id == competition.Id);
        return Task.CompletedTask;
    }
}