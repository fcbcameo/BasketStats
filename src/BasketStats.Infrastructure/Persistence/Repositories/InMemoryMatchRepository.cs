// A simple InMemoryMatchRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;

namespace BasketStats.Infrastructure.Persistence.Repositories;
public class InMemoryMatchRepository : IMatchRepository
{
    private static readonly List<Match> _matches = new();
    public Task AddAsync(Match match)
    {
        _matches.Add(match);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Match>> GetAllAsync()
    {
        return Task.FromResult(_matches.AsEnumerable());
    }

    Task IMatchRepository.DeleteAsync(Match match)
    {
        throw new NotImplementedException();
    }

    Task<Match?> IMatchRepository.GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}