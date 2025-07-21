using FluentAssertions;
using Xunit;

using BasketStats.Infrastructure.Services.CsvConverters;

namespace BasketStats.Infrastructure.Tests;

public class UnderScoreToZeroIntConverterTests
{
    private readonly UnderscoreToZeroIntConverter _converter;

    public UnderScoreToZeroIntConverterTests()
    {
        _converter = new UnderscoreToZeroIntConverter();
    }

    // Remove usage of 'Text' property on ConvertFromStringArgs, as it does not exist.
    // Instead, pass the string directly to ConvertFromString as in the other tests.

    [Fact]
    public void ConvertFromString_WithUnderscore_ShouldReturnZero()
    {
        // Act
        var result = _converter.ConvertFromString("_", null, null);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void ConvertFromString_WithValidNumber_ShouldReturnNumber()
    {
        // Act
        var result = _converter.ConvertFromString("25", null, null);

        // Assert
        result.Should().Be(25);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("100", 100)]
    [InlineData("-5", -5)]
    public void ConvertFromString_WithVariousNumbers_ShouldReturnCorrectValue(string input, int expected)
    {
        // Act
        var result = _converter.ConvertFromString(input, null, null);

        // Assert
        result.Should().Be(expected);
    }
}
