// src/BasketStats.Infrastructure/Persistence/Configurations/MatchConfiguration.cs
using BasketStats.Domain;
using BasketStats.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketStats.Infrastructure.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);

        // Competition -> Matches cascade
        builder.HasOne<Competition>()
               .WithMany()
               .HasForeignKey(m => m.CompetitionId)
               .OnDelete(DeleteBehavior.Cascade);

        // Map PlayerStats as owned collection via navigation property
        builder.OwnsMany(m => m.PlayerStats, b =>
        {
            b.WithOwner().HasForeignKey("MatchId");
            b.ToTable("PlayerStats");

            // match existing migration (int identity Id)
            b.Property<int>("Id").ValueGeneratedOnAdd();
            b.HasKey("Id");

            b.Property(ps => ps.PlayerId);
            b.Property(ps => ps.Points);
            b.Property(ps => ps.Assists);
            b.Property(ps => ps.Rebounds);
            b.Property(ps => ps.Minutes);
            b.Property(ps => ps.FieldGoalsMade);
            b.Property(ps => ps.FieldGoalsAttempted);
            b.Property(ps => ps.FreeThrowsMade);
            b.Property(ps => ps.FreeThrowsAttempted);
            b.Property(ps => ps.TwoPointersMade);
            b.Property(ps => ps.TwoPointersAttempted);
            b.Property(ps => ps.ThreePointersMade);
            b.Property(ps => ps.ThreePointersAttempted);
            b.Property(ps => ps.Turnovers);
            b.Property(ps => ps.Steals);
            b.Property(ps => ps.Blocks);
            b.Property(ps => ps.PersonalFouls);
            b.Property(ps => ps.OffensiveRebounds);
            b.Property(ps => ps.DefensiveRebounds);
        });

        // Use field access for the collection navigation
        builder.Navigation(m => m.PlayerStats).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}