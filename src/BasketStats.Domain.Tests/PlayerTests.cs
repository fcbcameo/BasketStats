using Xunit;
using FluentAssertions;
using BasketStats.Domain;

namespace BasketStats.Domain.Tests;

public class PlayerTests
{
    [Fact]
    public void Create_ValidName_ShouldCreatePlayer()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var player = Player.Create(name);

        // Assert
        player.Should().NotBeNull();
        player.Name.Should().Be(name);
        player.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_ValidName_ShouldGenerateUniqueIds()
    {
        // Arrange
        var name = "Test Player";

        // Act
        var player1 = Player.Create(name);
        var player2 = Player.Create(name);

        // Assert
        player1.Id.Should().NotBe(player2.Id);
    }

    [Theory]
    [InlineData("Michael Jordan")]
    [InlineData("LeBron James")]
    [InlineData("Stephen Curry")]
    [InlineData("Kobe Bryant")]
    public void Create_VariousValidNames_ShouldCreatePlayer(string name)
    {
        // Act
        var player = Player.Create(name);

        // Assert
        player.Should().NotBeNull();
        player.Name.Should().Be(name);
        player.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("X")]
    [InlineData("Player with very long name that should still work")]
    public void Create_EdgeCaseNames_ShouldCreatePlayer(string name)
    {
        // Act
        var player = Player.Create(name);

        // Assert
        player.Should().NotBeNull();
        player.Name.Should().Be(name);
    }
}