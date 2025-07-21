using Xunit;
using FluentAssertions;
using BasketStats.Domain;
using BasketStats.Domain.ValueObjects;

namespace BasketStats.Domain.Tests;

public class MatchTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateMatch()
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Now;
        var playerStats = new List<PlayerStats>
        {
            new PlayerStats { PlayerId = Guid.NewGuid(), Points = 25, Assists = 7 },
            new PlayerStats { PlayerId = Guid.NewGuid(), Points = 18, Assists = 5 }
        };

        // Act
        var match = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match.Should().NotBeNull();
        match.Id.Should().NotBe(Guid.Empty);
        match.CompetitionId.Should().Be(competitionId);
        match.MatchDate.Should().Be(matchDate);
        match.PlayerStats.Should().HaveCount(2);
    }

    [Fact]
    public void Create_WithEmptyPlayerStats_ShouldCreateMatchWithNoStats()
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Now;
        var playerStats = new List<PlayerStats>();

        // Act
        var match = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match.Should().NotBeNull();
        match.CompetitionId.Should().Be(competitionId);
        match.PlayerStats.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithMultiplePlayerStats_ShouldIncludeAllStats()
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Now;
        var player1Id = Guid.NewGuid();
        var player2Id = Guid.NewGuid();
        var player3Id = Guid.NewGuid();
        
        var playerStats = new List<PlayerStats>
        {
            new PlayerStats { PlayerId = player1Id, Points = 25, Assists = 7, Rebounds = 10 },
            new PlayerStats { PlayerId = player2Id, Points = 18, Assists = 5, Rebounds = 8 },
            new PlayerStats { PlayerId = player3Id, Points = 12, Assists = 3, Rebounds = 6 }
        };

        // Act
        var match = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match.PlayerStats.Should().HaveCount(3);
        match.PlayerStats.Should().Contain(s => s.PlayerId == player1Id && s.Points == 25);
        match.PlayerStats.Should().Contain(s => s.PlayerId == player2Id && s.Points == 18);
        match.PlayerStats.Should().Contain(s => s.PlayerId == player3Id && s.Points == 12);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds()
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Now;
        var playerStats = new List<PlayerStats>();

        // Act
        var match1 = Match.Create(competitionId, matchDate, playerStats);
        var match2 = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match1.Id.Should().NotBe(match2.Id);
    }

    [Fact]
    public void PlayerStats_ShouldBeReadOnly()
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Now;
        var playerStats = new List<PlayerStats>
        {
            new PlayerStats { PlayerId = Guid.NewGuid(), Points = 25 }
        };

        // Act
        var match = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match.PlayerStats.Should().BeAssignableTo<IReadOnlyCollection<PlayerStats>>();
        // The collection should be read-only, meaning we can't cast it back to a mutable list
        var readOnlyStats = match.PlayerStats;
        readOnlyStats.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("2024-01-01")]
    [InlineData("2024-06-15")]
    [InlineData("2024-12-31")]
    public void Create_WithDifferentDates_ShouldSetCorrectMatchDate(string dateString)
    {
        // Arrange
        var competitionId = Guid.NewGuid();
        var matchDate = DateTime.Parse(dateString);
        var playerStats = new List<PlayerStats>();

        // Act
        var match = Match.Create(competitionId, matchDate, playerStats);

        // Assert
        match.MatchDate.Should().Be(matchDate);
    }
}