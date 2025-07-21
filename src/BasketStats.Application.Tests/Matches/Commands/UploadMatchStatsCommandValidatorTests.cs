using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using BasketStats.Application.Matches.Commands.UploadMatchStats;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace BasketStats.Application.Tests.Matches.Commands;

public class UploadMatchStatsCommandValidatorTests
{
    private readonly UploadMatchStatsCommandValidator _validator;

    public UploadMatchStatsCommandValidatorTests()
    {
        _validator = new UploadMatchStatsCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_CompetitionId_Is_Empty()
    {
        // Arrange
        var mockFile = CreateMockCsvFile();
        var command = new UploadMatchStatsCommand(Guid.Empty, mockFile.Object);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CompetitionId);
    }

    //[Fact]
    //public void Should_Have_Error_When_CsvFile_Is_Null()
    //{
    //    // Arrange
    //    var command = new UploadMatchStatsCommand(Guid.NewGuid(), null);

    //    // Act & Assert
    //    var result = _validator.TestValidate(command);
    //    result.ShouldHaveValidationErrorFor(x => x.CsvFile);
    //}

    [Fact]
    public void Should_Have_Error_When_CsvFile_Is_Empty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);
        mockFile.Setup(f => f.ContentType).Returns("text/csv");
        
        var command = new UploadMatchStatsCommand(Guid.NewGuid(), mockFile.Object);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CsvFile);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Command()
    {
        // Arrange
        var mockFile = CreateMockCsvFile();
        var command = new UploadMatchStatsCommand(Guid.NewGuid(), mockFile.Object);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private Mock<IFormFile> CreateMockCsvFile()
    {
        var content = "Name,PTS,MIN\nPlayer1,20,30\n";
        var fileName = "test.csv";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(stream.Length);
        mockFile.Setup(f => f.ContentType).Returns("text/csv");
        mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

        return mockFile;
    }
}