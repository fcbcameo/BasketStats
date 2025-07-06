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
}