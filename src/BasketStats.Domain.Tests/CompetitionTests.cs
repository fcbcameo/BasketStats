using Xunit;
using FluentAssertions;
using BasketStats.Domain;

namespace BasketStats.Domain.Tests;

public class CompetitionTests
{
    [Fact]
    public void Create_ValidName_ShouldCreateCompetition()
    {
        // Arrange
        var name = "Basketball League 2024";

        // Act
        var competition = Competition.Create(name);

        // Assert
        competition.Should().NotBeNull();
        competition.Name.Should().Be(name);
        competition.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Act & Assert
        var action = () => Competition.Create(invalidName);
        action.Should().Throw<ArgumentException>()
              .WithMessage("Competition name cannot be empty. (Parameter 'name')");
    }

    [Fact]
    public void Create_ValidName_ShouldGenerateUniqueIds()
    {
        // Arrange
        var name = "Test Competition";

        // Act
        var competition1 = Competition.Create(name);
        var competition2 = Competition.Create(name);

        // Assert
        competition1.Id.Should().NotBe(competition2.Id);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Basketball")]
    [InlineData("Very Long Competition Name That Should Still Work")]
    [InlineData("Competition with Numbers 123")]
    [InlineData("Competition-with-special_characters")]
    public void Create_VariousValidNames_ShouldCreateCompetition(string name)
    {
        // Act
        var competition = Competition.Create(name);

        // Assert
        competition.Should().NotBeNull();
        competition.Name.Should().Be(name);
        competition.Id.Should().NotBe(Guid.Empty);
    }
}