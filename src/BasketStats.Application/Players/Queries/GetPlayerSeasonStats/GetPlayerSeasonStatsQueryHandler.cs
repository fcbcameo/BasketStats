// src/BasketStats.Application/Players/Queries/GetPlayerSeasonStats/GetPlayerSeasonStatsQueryHandler.cs
using BasketStats.Application.DTOs;
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Players.Queries.GetPlayerSeasonStats;

public class GetPlayerSeasonStatsQueryHandler : IRequestHandler<GetPlayerSeasonStatsQuery, PlayerSeasonStatsDto?>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerSeasonStatsQueryHandler(IMatchRepository matchRepository, IPlayerRepository playerRepository)
    {
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
    }

    public async Task<PlayerSeasonStatsDto?> Handle(GetPlayerSeasonStatsQuery request, CancellationToken cancellationToken)
    {
        // 1. Get the player
        var player = await _playerRepository.GetByIdAsync(request.PlayerId);
        if (player is null) return null; // Player not found

        // 2. Get all matches
        var allMatches = await _matchRepository.GetAllAsync();

        // 3. Filter the stats for this specific player
        var relevantStats = allMatches
            // Optional: Filter by competition
            .Where(m => !request.CompetitionId.HasValue || m.CompetitionId == request.CompetitionId.Value)
            .SelectMany(m => m.PlayerStats) // Flatten the list of lists of stats
            .Where(s => s.PlayerId == request.PlayerId)
            .ToList();

        if (!relevantStats.Any())
        {
            // Player exists but has no stats for this filter
            return new PlayerSeasonStatsDto { PlayerId = player.Id, PlayerName = player.Name, GamesPlayed = 0 };
        }

        // 4. Aggregate the stats
        int total3PM = relevantStats.Sum(s => s.ThreePointersMade);
        int total3PA = relevantStats.Sum(s => s.ThreePointersAttempted);
        int totalFGM = relevantStats.Sum(s => s.FieldGoalsMade);
        int totalFGA = relevantStats.Sum(s => s.FieldGoalsAttempted);
        int totalFTM = relevantStats.Sum(s => s.FreeThrowsMade);
        int totalFTA = relevantStats.Sum(s => s.FreeThrowsAttempted);
        int totalTwoPM = relevantStats.Sum(s => s.TwoPointersMade);
        int totalTwoPA = relevantStats.Sum(s => s.TwoPointersAttempted);
        int totalTurnovers = relevantStats.Sum(s => s.Turnovers);
        int totalSteals = relevantStats.Sum(s => s.Steals);
        int totalBlocks = relevantStats.Sum(s => s.Blocks);
        int totalPersonalFouls = relevantStats.Sum(s => s.PersonalFouls);
        int totalOffensiveRebounds = relevantStats.Sum(s => s.OffensiveRebounds);
        int totalDefensiveRebounds = relevantStats.Sum(s => s.DefensiveRebounds);
        int totalMinutes = relevantStats.Sum(s => s.Minutes);
        int totalPoints = relevantStats.Sum(s => s.Points);
        int totalAssists = relevantStats.Sum(s => s.Assists);
        int totalGames = relevantStats.Count;
        int totalPlayers = relevantStats.Count;
        int totalRebounds = totalOffensiveRebounds + totalDefensiveRebounds;
        //TODO: add info?

        // 5. Create the response DTO
        var result = new PlayerSeasonStatsDto
        {
            PlayerId = player.Id,
            PlayerName = player.Name,
            GamesPlayed = relevantStats.Count,
            TotalPoints = relevantStats.Sum(s => s.Points),
            TotalAssists = relevantStats.Sum(s => s.Assists),
            TotalRebounds = relevantStats.Sum(s => s.Rebounds),
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
            TotalGames = totalGames,

            // Handle division by zero when calculating percentages
            ThreePointPercentage = total3PA > 0 ? (double)total3PM / total3PA * 100 : 0, // Percentage of 3-pointers made
            FieldGoalPercentage = totalFGA > 0 ? (double)totalFGM / totalFGA * 100 : 0, // Percentage of field goals made
            FreeThrowPercentage = totalFTA > 0 ? (double)totalFTM / totalFTA * 100 : 0, // Percentage of free throws made
            TwoPointPercentage = totalTwoPA > 0 ? (double)totalTwoPM / totalTwoPA * 100 : 0 // Percentage of two-pointers made
            


        };

        return result;
    }
}