// src/BasketStats.Infrastructure/Persistence/Repositories/EfPlayerRepository.cs
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BasketStats.Infrastructure.Persistence.Repositories;

public class EfPlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public EfPlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Player player)
    {
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<Player?> GetByNameAsync(string name)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Name == name);
    }
}
