// src/BasketStats.Domain/Repositories/IPlayerRepository.cs
namespace BasketStats.Domain.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByNameAsync(string name);
    Task AddAsync(Player player);
    Task<Player?> GetByIdAsync(Guid id);
    Task<IEnumerable<Player>> GetAllAsync();
}