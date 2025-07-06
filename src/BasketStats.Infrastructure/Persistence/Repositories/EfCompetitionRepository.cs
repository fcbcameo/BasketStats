// src/BasketStats.Infrastructure/Persistence/Repositories/EfCompetitionRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class EfCompetitionRepository : ICompetitionRepository
{
    private readonly ApplicationDbContext _context;

    public EfCompetitionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Competition competition)
    {
        await _context.Competitions.AddAsync(competition);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Competition>> GetAllAsync()
    {
        return await _context.Competitions.ToListAsync();
    }

    public async Task<Competition?> GetByIdAsync(Guid id)
    {
        return await _context.Competitions.FindAsync(id);
    }
}