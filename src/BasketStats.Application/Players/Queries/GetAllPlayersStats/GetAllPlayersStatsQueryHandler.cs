// src/BasketStats.Application/Players/Queries/GetAllPlayersStats/GetAllPlayersStatsQueryHandler.cs
using BasketStats.Application.DTOs;
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Players.Queries.GetAllPlayersStats;

public class GetAllPlayersStatsQueryHandler : IRequestHandler<GetAllPlayersStatsQuery, IEnumerable<PlayerSeasonStatsDto>>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;

    public GetAllPlayersStatsQueryHandler(IMatchRepository matchRepository, IPlayerRepository playerRepository)
    {
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<PlayerSeasonStatsDto>> Handle(GetAllPlayersStatsQuery request, CancellationToken cancellationToken)
    {
        // 1. Get all players
        var allPlayers = await _playerRepository.GetAllAsync();
        
        // 2. Get all matches (with optional competition filter)
        var allMatches = await _matchRepository.GetAllAsync();
        var filteredMatches = allMatches
            .Where(m => !request.CompetitionId.HasValue || m.CompetitionId == request.CompetitionId.Value)
            .ToList();

        var results = new List<PlayerSeasonStatsDto>();

        // 3. Process each player
        foreach (var player in allPlayers)
        {
            var playerStats = filteredMatches
                .SelectMany(m => m.PlayerStats)
                .Where(s => s.PlayerId == player.Id)
                .ToList();

            if (!playerStats.Any())
            {
                // Player exists but has no stats for this filter
                results.Add(new PlayerSeasonStatsDto 
                { 
                    PlayerId = player.Id, 
                    PlayerName = player.Name, 
                    GamesPlayed = 0 
                });
                continue;
            }

            // 4. Aggregate the stats for this player
            int total3PM = playerStats.Sum(s => s.ThreePointersMade);
            int total3PA = playerStats.Sum(s => s.ThreePointersAttempted);
            int totalFGM = playerStats.Sum(s => s.FieldGoalsMade);
            int totalFGA = playerStats.Sum(s => s.FieldGoalsAttempted);
            int totalFTM = playerStats.Sum(s => s.FreeThrowsMade);
            int totalFTA = playerStats.Sum(s => s.FreeThrowsAttempted);
            int totalTwoPM = playerStats.Sum(s => s.TwoPointersMade);
            int totalTwoPA = playerStats.Sum(s => s.TwoPointersAttempted);

            var playerSeasonStats = new PlayerSeasonStatsDto
            {
                PlayerId = player.Id,
                PlayerName = player.Name,
                GamesPlayed = playerStats.Count,
                TotalPoints = playerStats.Sum(s => s.Points),
                TotalAssists = playerStats.Sum(s => s.Assists),
                TotalRebounds = playerStats.Sum(s => s.Rebounds),
                TotalThreePointersMade = total3PM,
                TotalThreePointersAttempted = total3PA,
                TotalFieldGoalsMade = totalFGM,
                TotalFieldGoalsAttempted = totalFGA,
                TotalFreeThrowsMade = totalFTM,
                TotalFreeThrowsAttempted = totalFTA,
                TotalTwoPointersMade = totalTwoPM,
                TotalTwoPointersAttempted = totalTwoPA,
                TotalTurnovers = playerStats.Sum(s => s.Turnovers),
                TotalSteals = playerStats.Sum(s => s.Steals),
                TotalBlocks = playerStats.Sum(s => s.Blocks),
                TotalPersonalFouls = playerStats.Sum(s => s.PersonalFouls),
                TotalOffensiveRebounds = playerStats.Sum(s => s.OffensiveRebounds),
                TotalDefensiveRebounds = playerStats.Sum(s => s.DefensiveRebounds),
                TotalMinutes = playerStats.Sum(s => s.Minutes),
                TotalGames = playerStats.Count,

                // Calculate percentages (handle division by zero)
                ThreePointPercentage = total3PA > 0 ? (double)total3PM / total3PA * 100 : 0,
                FieldGoalPercentage = totalFGA > 0 ? (double)totalFGM / totalFGA * 100 : 0,
                FreeThrowPercentage = totalFTA > 0 ? (double)totalFTM / totalFTA * 100 : 0,
                TwoPointPercentage = totalTwoPA > 0 ? (double)totalTwoPM / totalTwoPA * 100 : 0
            };

            results.Add(playerSeasonStats);
        }

        return results.Where(r => r.GamesPlayed > 0); // Only return players with stats
    }
}