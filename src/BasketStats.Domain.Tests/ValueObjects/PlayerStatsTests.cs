using Xunit;
using FluentAssertions;
using BasketStats.Domain.ValueObjects;

namespace BasketStats.Domain.Tests.ValueObjects;

public class PlayerStatsTests
{
    [Fact]
    public void PlayerStats_WithValidValues_ShouldCreateCorrectly()
    {
        // Arrange & Act
        var playerStats = new PlayerStats
        {
            PlayerId = Guid.NewGuid(),
            Points = 25,
            Assists = 7,
            Rebounds = 10,
            Minutes = 35,
            FieldGoalsMade = 10,
            FieldGoalsAttempted = 18,
            FreeThrowsMade = 5,
            FreeThrowsAttempted = 6,
            ThreePointersMade = 3,
            ThreePointersAttempted = 7,
            TwoPointersMade = 7,
            TwoPointersAttempted = 11,
            Turnovers = 3,
            Steals = 2,
            Blocks = 1,
            PersonalFouls = 4,
            OffensiveRebounds = 2,
            DefensiveRebounds = 8
        };

        // Assert
        playerStats.Points.Should().Be(25);
        playerStats.Assists.Should().Be(7);
        playerStats.Rebounds.Should().Be(10);
        playerStats.Minutes.Should().Be(35);
        playerStats.FieldGoalsMade.Should().Be(10);
        playerStats.FieldGoalsAttempted.Should().Be(18);
        playerStats.ThreePointersMade.Should().Be(3);
        playerStats.ThreePointersAttempted.Should().Be(7);
    }

    [Fact]
    public void PlayerStats_Records_ShouldBeEqual_WhenAllPropertiesAreEqual()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var stats1 = new PlayerStats
        {
            PlayerId = playerId,
            Points = 25,
            Assists = 7,
            Rebounds = 10,
            Minutes = 35,
            FieldGoalsMade = 10,
            FieldGoalsAttempted = 18
        };

        var stats2 = new PlayerStats
        {
            PlayerId = playerId,
            Points = 25,
            Assists = 7,
            Rebounds = 10,
            Minutes = 35,
            FieldGoalsMade = 10,
            FieldGoalsAttempted = 18
        };

        // Act & Assert
        stats1.Should().Be(stats2);
        stats1.GetHashCode().Should().Be(stats2.GetHashCode());
    }

    [Fact]
    public void PlayerStats_Records_ShouldNotBeEqual_WhenPropertiesDiffer()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var stats1 = new PlayerStats
        {
            PlayerId = playerId,
            Points = 25,
            Assists = 7
        };

        var stats2 = new PlayerStats
        {
            PlayerId = playerId,
            Points = 20, // Different value
            Assists = 7
        };

        // Act & Assert
        stats1.Should().NotBe(stats2);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(25, 7, 10, 35)]
    [InlineData(100, 20, 15, 48)]
    public void PlayerStats_WithVariousValues_ShouldSetCorrectly(int points, int assists, int rebounds, int minutes)
    {
        // Arrange & Act
        var playerStats = new PlayerStats
        {
            PlayerId = Guid.NewGuid(),
            Points = points,
            Assists = assists,
            Rebounds = rebounds,
            Minutes = minutes
        };

        // Assert
        playerStats.Points.Should().Be(points);
        playerStats.Assists.Should().Be(assists);
        playerStats.Rebounds.Should().Be(rebounds);
        playerStats.Minutes.Should().Be(minutes);
    }
}