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
    public double EffectiveFieldGoalPercentage => TotalFieldGoalsAttempted > 0 ? (TotalFieldGoalsMade + 0.5 * TotalThreePointersMade) / TotalFieldGoalsAttempted * 100 : 0; // eFG%
    public int TrueShootingAttempts => TotalFieldGoalsAttempted + 0.44 * TotalFreeThrowsAttempted > 0 ? (int)(TotalFieldGoalsAttempted + 0.44 * TotalFreeThrowsAttempted) : 0; // TSA
    public double TrueShootingPercentage => TrueShootingAttempts > 0 ? (double)TotalPoints / (2 * TrueShootingAttempts) * 100 : 0; // TS%
    public double AssistToTurnoverRatio => TotalTurnovers > 0 ? (double)TotalAssists / TotalTurnovers : TotalAssists; // Avoid division by zero


    // You can add many more stats here (e.g., PPG, RPG, other percentages)
}