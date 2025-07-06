// src/BasketStats.Application/Matches/Commands/UploadMatchStats/UploadMatchStatsCommandHandler.cs
using BasketStats.Application.Services;
using BasketStats.Domain;
using BasketStats.Domain.Repositories;
using MediatR;

namespace BasketStats.Application.Matches.Commands.UploadMatchStats;

public class UploadMatchStatsCommandHandler : IRequestHandler<UploadMatchStatsCommand, Guid>
{
    private readonly ICsvParser _csvParser;
    private readonly IMatchRepository _matchRepository;

    // In a real scenario, you'd also inject IPlayerRepository
    public UploadMatchStatsCommandHandler(ICsvParser csvParser, IMatchRepository matchRepository)
    {
        _csvParser = csvParser;
        _matchRepository = matchRepository;
    }

    public async Task<Guid> Handle(UploadMatchStatsCommand request, CancellationToken cancellationToken)
    {
        // 1. Parse the CSV file to get the raw stats
        var playerStats = await _csvParser.ParseMatchStatsAsync(request.CsvFile);

        // Here you would add logic to:
        // - Loop through stats from the CSV
        // - Use IPlayerRepository to find existing players or create new ones
        // - Collect the final PlayerStats value objects with correct PlayerIds

        // 2. Create the Match aggregate
        var match = Match.Create(request.CompetitionId, DateTime.UtcNow, playerStats);

        // 3. Save the new match
        await _matchRepository.AddAsync(match);

        return match.Id;
    }
}