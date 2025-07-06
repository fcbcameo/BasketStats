// src/BasketStats.Domain/ValueObjects/PlayerStats.cs
namespace BasketStats.Domain.ValueObjects;

// A record is a great choice for a value object
public record PlayerStats
{
    public Guid PlayerId { get; init; }
    public int Points { get; init; }
    public int Assists { get; init; }
    public int Rebounds { get; init; }
    public  int Minutes { get; set; }
    public int FieldGoalsMade { get; init; }
    public int FieldGoalsAttempted { get; init; }
    public int FreeThrowsMade { get; init; }    
    public int FreeThrowsAttempted { get; init; }
    public int TwoPointersMade { get; init; }
    public int TwoPointersAttempted { get; init; }
    public int ThreePointersMade { get; init; }
    public int ThreePointersAttempted { get; init; }

    public int Turnovers { get; init; }
    public int Steals { get; init; }
    public int Blocks { get; init; }
    public int PersonalFouls { get; init; }
    public int OffensiveRebounds { get; init; }
    public int DefensiveRebounds { get; init; }
    


    
}