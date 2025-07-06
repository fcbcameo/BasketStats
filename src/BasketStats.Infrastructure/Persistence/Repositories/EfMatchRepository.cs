// src/BasketStats.Infrastructure/Persistence/Repositories/EfMatchRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class EfMatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;

    public EfMatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Match match)
    {
        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Match>> GetAllAsync()
    {
        return await _context.Matches.ToListAsync();
    }
}
