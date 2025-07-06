// src/BasketStats.Application/DTOs/PlayerSeasonStatsDto.cs
namespace BasketStats.Application.DTOs;

public class PlayerSeasonStatsDto
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int GamesPlayed { get; set; }

    // Totals
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
    public double PersonalFoulsPerGame => GamesPlayed > 0 ? (double)TotalPersonalFouls / GamesPlayed : 0;
    public double OffensiveReboundsPerGame => GamesPlayed > 0 ? (double)TotalOffensiveRebounds / GamesPlayed : 0;
    public double DefensiveReboundsPerGame => GamesPlayed > 0 ? (double)TotalDefensiveRebounds / GamesPlayed : 0;
    public double MinutesPerGame => GamesPlayed > 0 ? (double)TotalMinutes / GamesPlayed : 0;
    public double Efficiency => (TotalPoints + TotalRebounds + TotalAssists + TotalSteals + TotalBlocks - TotalTurnovers - TotalPersonalFouls) / (double)GamesPlayed;
    public double UsageRate => GamesPlayed > 0 ? (TotalFieldGoalsAttempted + TotalFreeThrowsAttempted + TotalTurnovers) / (double)(GamesPlayed * 20) : 0;
    public double WinShares => GamesPlayed > 0 ? (TotalPoints + TotalRebounds + TotalAssists + TotalSteals + TotalBlocks - TotalTurnovers - TotalPersonalFouls) / (double)(GamesPlayed * 10) : 0;
    public double PlayerEfficiencyRating => GamesPlayed > 0 ? (TotalPoints + TotalRebounds + TotalAssists + TotalSteals + TotalBlocks - TotalTurnovers - TotalPersonalFouls) / (double)(GamesPlayed * 15) : 0;
    public double TrueShootingPercentage => GamesPlayed > 0 ? (TotalPoints / (2 * (TotalFieldGoalsAttempted + 0.44 * TotalFreeThrowsAttempted))) : 0;
    public double OffensiveRating => GamesPlayed > 0 ? (TotalPoints + TotalAssists * 0.5 + TotalRebounds * 0.3) / (TotalFieldGoalsAttempted + TotalTurnovers) : 0;
    public double DefensiveRating => GamesPlayed > 0 ? (TotalPoints + TotalRebounds * 0.3 + TotalSteals * 0.5 + TotalBlocks * 0.7) / (TotalFieldGoalsAttempted + TotalTurnovers) : 0;
    public double AssistToTurnoverRatio => TotalTurnovers > 0 ? (double)TotalAssists / TotalTurnovers : TotalAssists; // Avoid division by zero
    public double PointsPerAssist => TotalAssists > 0 ? (double)TotalPoints / TotalAssists : TotalPoints; // Avoid division by zero
    public double PointsPerRebound => TotalRebounds > 0 ? (double)TotalPoints / TotalRebounds : TotalPoints; // Avoid division by zero
    public double PointsPerTurnover => TotalTurnovers > 0 ? (double)TotalPoints / TotalTurnovers : TotalPoints; // Avoid division by zero
    public double PointsPerFieldGoalAttempt => TotalFieldGoalsAttempted > 0 ? (double)TotalPoints / TotalFieldGoalsAttempted : TotalPoints; // Avoid division by zero
    public double PointsPerFreeThrowAttempt => TotalFreeThrowsAttempted > 0 ? (double)TotalPoints / TotalFreeThrowsAttempted : TotalPoints; // Avoid division by zero
    public double PointsPerTwoPointerAttempt => TotalTwoPointersAttempted > 0 ? (double)TotalPoints / TotalTwoPointersAttempted : TotalPoints; // Avoid division by zero
    public double PointsPerThreePointerAttempt => TotalThreePointersAttempted > 0 ? (double)TotalPoints / TotalThreePointersAttempted : TotalPoints; // Avoid division by zero
    public double PointsPerOffensiveRebound => TotalOffensiveRebounds > 0 ? (double)TotalPoints / TotalOffensiveRebounds : TotalPoints; // Avoid division by zero
    public double PointsPerDefensiveRebound => TotalDefensiveRebounds > 0 ? (double)TotalPoints / TotalDefensiveRebounds : TotalPoints; // Avoid division by zero
    public double PointsPerMinute => TotalMinutes > 0 ? (double)TotalPoints / TotalMinutes : TotalPoints; // Avoid division by zero
    

    // You can add many more stats here (e.g., PPG, RPG, other percentages)
}