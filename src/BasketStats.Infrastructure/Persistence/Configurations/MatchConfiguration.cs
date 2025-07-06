// src/BasketStats.Infrastructure/Persistence/Configurations/MatchConfiguration.cs
using BasketStats.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketStats.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        // Tell EF Core that PlayerStats is a collection of owned entities (value objects).
        // This will create a "PlayerStats" table with a foreign key back to the "Matches" table.
        builder.OwnsMany(m => m.PlayerStats, sa =>
        {
            sa.WithOwner().HasForeignKey("MatchId");
            sa.Property<int>("Id"); // Add a shadow primary key
            sa.HasKey("Id");
        });
    }
}