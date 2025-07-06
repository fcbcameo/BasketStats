// src/BasketStats.Application/Matches/Commands/UploadMatchStats/UploadMatchStatsCommandHandler.cs
using BasketStats.Application.Services;
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using BasketStats.Domain.ValueObjects; // Add this
using MediatR;

namespace BasketStats.Application.Matches.Commands.UploadMatchStats;

public class UploadMatchStatsCommandHandler : IRequestHandler<UploadMatchStatsCommand, Guid>
{
    private readonly ICsvParser _csvParser;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository; // Add repository

    public UploadMatchStatsCommandHandler(
        ICsvParser csvParser,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository) // Inject it
    {
        _csvParser = csvParser;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository; // Assign it
    }

    public async Task<Guid> Handle(UploadMatchStatsCommand request, CancellationToken cancellationToken)
    {
        // 1. Parse the CSV to get raw data
        var parsedStats = await _csvParser.ParseMatchStatsAsync(request.CsvFile);
        var finalPlayerStats = new List<PlayerStats>();

        // 2. Process each player record
        foreach (var parsedStat in parsedStats)
        {
            // Find player by name
            var player = await _playerRepository.GetByNameAsync(parsedStat.PlayerName);

            // If player doesn't exist, create and save them
            if (player is null)
            {
                player = Player.Create(parsedStat.PlayerName);
                await _playerRepository.AddAsync(player);
            }

            // Create the final PlayerStats value object with the correct PlayerId
            var stats = new PlayerStats
            {
                PlayerId = player.Id,
                Points = parsedStat.Points,
                Assists = parsedStat.Assists,
                Rebounds = parsedStat.Rebounds,
                ThreePointersMade = parsedStat.ThreePointersMade,
                ThreePointersAttempted = parsedStat.ThreePointersAttempted,
                FieldGoalsMade = parsedStat.FieldGoalsMade,
                FieldGoalsAttempted = parsedStat.FieldGoalsAttempted,
                FreeThrowsMade = parsedStat.FreeThrowsMade,
                FreeThrowsAttempted = parsedStat.FreeThrowsAttempted,
                TwoPointersMade = parsedStat.TwoPointersMade,
                TwoPointersAttempted = parsedStat.TwoPointersAttempted,
                Turnovers = parsedStat.Turnovers,
                Steals = parsedStat.Steals,
                Blocks = parsedStat.Blocks,
                PersonalFouls = parsedStat.PersonalFouls,
                OffensiveRebounds = parsedStat.OffensiveRebounds,
                DefensiveRebounds = parsedStat.DefensiveRebounds,
                Minutes = parsedStat.Minutes

            };
            finalPlayerStats.Add(stats);

        }

        // 3. Create the Match aggregate with the processed stats
        var match = Match.Create(request.CompetitionId, DateTime.UtcNow, finalPlayerStats);

        // 4. Save the new match
        await _matchRepository.AddAsync(match);

        return match.Id;
    }
}