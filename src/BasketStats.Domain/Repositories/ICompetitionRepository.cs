namespace BasketStats.Domain.Repositories;

public interface ICompetitionRepository
{
    Task AddAsync(Competition competition);
    Task<Competition?> GetByIdAsync(Guid id);
    Task<IEnumerable<Competition>> GetAllAsync();
    Task DeleteAsync(Competition competition);
}
