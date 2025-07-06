// src/BasketStats.Infrastructure/Persistence/ApplicationDbContext.cs
using BasketStats.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BasketStats.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define the database sets for our aggregate roots
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This tells EF Core to look for all IEntityTypeConfiguration classes
        // in the current assembly and apply them. We will create one next.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}