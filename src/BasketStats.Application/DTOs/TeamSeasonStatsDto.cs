// src/BasketStats.Application/DTOs/TeamSeasonStatsDto.cs
namespace BasketStats.Application.DTOs;

public class TeamSeasonStatsDto
{
    public int GamesPlayed { get; set; }

    public int TotalPoints { get; set; }
    public int TotalAssists { get; set; }
    public int TotalRebounds { get; set; }
    public int TotalThreePointersMade { get; set; }
    public int TotalThreePointersAttempted { get; set; }
    public int TotalFieldGoalsMade { get; set; }
    public int TotalFieldGoalsAttempted { get; set; }
    public int TotalFreeThrowsMade { get; set; }
    public int TotalFreeThrowsAttempted { get; set; }
    public int TotalTwoPointersMade { get; set; }
    public int TotalTwoPointersAttempted { get; set; }
    public int TotalTurnovers { get; set; }
    public int TotalSteals { get; set; }
    public int TotalBlocks { get; set; }
    public int TotalPersonalFouls { get; set; }
    public int TotalOffensiveRebounds { get; set; }
    public int TotalDefensiveRebounds { get; set; }
    public int TotalMinutes { get; set; }
    public int TotalGames { get; set; }


    // Calculated Percentages //TODO verify what is really needed
    public double ThreePointPercentage { get; set; }
    public double FieldGoalPercentage { get; set; }
    public double FreeThrowPercentage { get; set; }
    public double TwoPointPercentage { get; set; }
    public double AssistsPerGame => GamesPlayed > 0 ? (double)TotalAssists / GamesPlayed : 0;
    public double PointsPerGame => GamesPlayed > 0 ? (double)TotalPoints / GamesPlayed : 0;
    public double ReboundsPerGame => GamesPlayed > 0 ? (double)TotalRebounds / GamesPlayed : 0;
    public double TurnoversPerGame => GamesPlayed > 0 ? (double)TotalTurnovers / GamesPlayed : 0;
    public double StealsPerGame => GamesPlayed > 0 ? (double)TotalSteals / GamesPlayed : 0;
    public double BlocksPerGame => GamesPlayed > 0 ? (double)TotalBlocks / GamesPlayed : 0;
    public double OffensiveReboundsPerGame => GamesPlayed > 0 ? (double)TotalOffensiveRebounds / GamesPlayed : 0;
    public double DefensiveReboundsPerGame => GamesPlayed > 0 ? (double)TotalDefensiveRebounds / GamesPlayed : 0;
    

    // You can add many more Team stats here (e.g., PPG, RPG, other percentages)
}