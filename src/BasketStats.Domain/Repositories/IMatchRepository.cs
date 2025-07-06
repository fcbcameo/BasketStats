// src/BasketStats.Domain/Repositories/IMatchRepository.cs
namespace BasketStats.Domain.Repositories;

public interface IMatchRepository
{
    Task AddAsync(Match match);
}