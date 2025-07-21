using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using BasketStats.Application.Competitions.Commands.CreateCompetition;

namespace BasketStats.Application.Tests.Competitions.Commands;

public class CreateCompetitionCommandValidatorTests
{
    private readonly CreateCompetitionCommandValidator _validator;

    public CreateCompetitionCommandValidatorTests()
    {
        _validator = new CreateCompetitionCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateCompetitionCommand("");

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        // Arrange
        var command = new CreateCompetitionCommand(null);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Whitespace()
    {
        // Arrange
        var command = new CreateCompetitionCommand("   ");

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        // Arrange
        var command = new CreateCompetitionCommand("Valid Competition Name");

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Basketball League 2024")]
    [InlineData("Super Long Competition Name That Still Should Be Valid")]
    public void Should_Not_Have_Error_When_Name_Has_Valid_Values(string name)
    {
        // Arrange
        var command = new CreateCompetitionCommand(name);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}