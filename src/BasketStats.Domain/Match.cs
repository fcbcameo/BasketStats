// src/BasketStats.Domain/Match.cs
using BasketStats.Domain.ValueObjects;

namespace BasketStats.Domain;

public class Match
{
    public Guid Id { get; private set; }
    public Guid CompetitionId { get; private set; }
    public DateTime MatchDate { get; private set; }
    private readonly List<PlayerStats> _playerStats = new();
    public IReadOnlyCollection<PlayerStats> PlayerStats => _playerStats.AsReadOnly();

    private Match() { }

    public static Match Create(Guid competitionId, DateTime matchDate, IEnumerable<PlayerStats> stats)
    {
        var match = new Match
        {
            Id = Guid.NewGuid(),
            CompetitionId = competitionId,
            MatchDate = matchDate
        };

        match._playerStats.AddRange(stats);
        return match;
    }
}