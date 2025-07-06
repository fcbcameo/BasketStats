// src/BasketStats.Application/Teams/Queries/GetTeamSeasonStats/GetTeamSeasonStatsQueryHandler.cs
using BasketStats.Application.DTOs;
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Teams.Queries.GetTeamSeasonStats;

public class GetTeamSeasonStatsQueryHandler : IRequestHandler<GetTeamSeasonStatsQuery, TeamSeasonStatsDto>
{
    private readonly IMatchRepository _matchRepository;

    public GetTeamSeasonStatsQueryHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<TeamSeasonStatsDto> Handle(GetTeamSeasonStatsQuery request, CancellationToken cancellationToken)
    {
        // 1. Get all matches, optionally filtering by competition
        var allMatches = await _matchRepository.GetAllAsync();
        var relevantMatches = allMatches
            .Where(m => !request.CompetitionId.HasValue || m.CompetitionId == request.CompetitionId.Value)
            .ToList();

        if (!relevantMatches.Any())
        {
            return new TeamSeasonStatsDto(); // Return empty stats if no matches found
        }

        // 2. Flatten all player stats from all relevant matches into a single list
        var allPlayerStats = relevantMatches.SelectMany(m => m.PlayerStats).ToList();

        // 4. Aggregate the stats
        int gamesPlayed = relevantMatches.Count;
        int total3PM = allPlayerStats.Sum(s => s.ThreePointersMade);
        int total3PA = allPlayerStats.Sum(s => s.ThreePointersAttempted);
        int totalFGM = allPlayerStats.Sum(s => s.FieldGoalsMade);
        int totalFGA = allPlayerStats.Sum(s => s.FieldGoalsAttempted);
        int totalFTM = allPlayerStats.Sum(s => s.FreeThrowsMade);
        int totalFTA = allPlayerStats.Sum(s => s.FreeThrowsAttempted);
        int totalTwoPM = allPlayerStats.Sum(s => s.TwoPointersMade);
        int totalTwoPA = allPlayerStats.Sum(s => s.TwoPointersAttempted);
        int totalTurnovers = allPlayerStats.Sum(s => s.Turnovers);
        int totalSteals = allPlayerStats.Sum(s => s.Steals);
        int totalBlocks = allPlayerStats.Sum(s => s.Blocks);
        int totalPersonalFouls = allPlayerStats.Sum(s => s.PersonalFouls);
        int totalOffensiveRebounds = allPlayerStats.Sum(s => s.OffensiveRebounds);
        int totalDefensiveRebounds = allPlayerStats.Sum(s => s.DefensiveRebounds);
        int totalMinutes = allPlayerStats.Sum(s => s.Minutes);
        int totalPoints = allPlayerStats.Sum(s => s.Points);
        int totalAssists = allPlayerStats.Sum(s => s.Assists);
        
        
        int totalRebounds = totalOffensiveRebounds + totalDefensiveRebounds;
        //TODO: add info?

        // 5. Create the response DTO
        var result = new TeamSeasonStatsDto
        {
            GamesPlayed = gamesPlayed,
            TotalPoints = totalPoints,
            TotalAssists = totalAssists,
            TotalRebounds = totalRebounds,
            TotalThreePointersMade = total3PM,
            TotalThreePointersAttempted = total3PA,

            TotalFieldGoalsMade = totalFGM,
            TotalFieldGoalsAttempted = totalFGA,
            TotalFreeThrowsMade = totalFTM,
            TotalFreeThrowsAttempted = totalFTA,
            TotalTwoPointersMade = totalTwoPM,
            TotalTwoPointersAttempted = totalTwoPA,
            TotalTurnovers = totalTurnovers,
            TotalSteals = totalSteals,
            TotalBlocks = totalBlocks,
            TotalPersonalFouls = totalPersonalFouls,
            TotalOffensiveRebounds = totalOffensiveRebounds,
            TotalDefensiveRebounds = totalDefensiveRebounds,
            TotalMinutes = totalMinutes,
            TotalGames = gamesPlayed,

            // Handle division by zero when calculating percentages
            ThreePointPercentage = total3PA > 0 ? (double)total3PM / total3PA * 100 : 0, // Percentage of 3-pointers made
            FieldGoalPercentage = totalFGA > 0 ? (double)totalFGM / totalFGA * 100 : 0, // Percentage of field goals made
            FreeThrowPercentage = totalFTA > 0 ? (double)totalFTM / totalFTA * 100 : 0, // Percentage of free throws made
            TwoPointPercentage = totalTwoPA > 0 ? (double)totalTwoPM / totalTwoPA * 100 : 0 // Percentage of two-pointers made
        };

        return result;
    }
}