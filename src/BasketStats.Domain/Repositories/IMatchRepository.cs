// src/BasketStats.Domain/Repositories/IMatchRepository.cs
namespace BasketStats.Domain.Repositories;

public interface IMatchRepository
{
    Task AddAsync(Match match);
    Task<IEnumerable<Match>> GetAllAsync();
    Task<Match?> GetByIdAsync(Guid id); 
    Task DeleteAsync(Match match);      
}