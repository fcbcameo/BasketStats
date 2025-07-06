// src/BasketStats.Infrastructure/Persistence/Repositories/InMemoryPlayerRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private static readonly List<Player> _players = new();

    public Task AddAsync(Player player)
    {
        _players.Add(player);
        return Task.CompletedTask;
    }

    public Task<Player?> GetByNameAsync(string name)
    {
        var player = _players.FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(player);
    }

    // Helper method for testing
    public Task<IEnumerable<Player>> GetAllAsync()
    {
        return Task.FromResult(_players.AsEnumerable());
    }
}