// src/BasketStats.Application/DTOs/ParsedPlayerStat.cs
namespace BasketStats.Application.DTOs;

public class ParsedPlayerStat
{
    public string PlayerName { get; set; } = string.Empty;
    public int Points { get; init; }
    public int Assists { get; init; }
    public int Rebounds { get; init; }
    public int Minutes { get; set; }
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