using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using BasketStats.Infrastructure.Services;

namespace BasketStats.Infrastructure.Tests.Services;

public class CsvParserServiceTests
{
    private readonly CsvParserService _csvParserService;

    public CsvParserServiceTests()
    {
        _csvParserService = new CsvParserService();
    }

    [Fact]
    public async Task ParseMatchStatsAsync_ValidCsvFile_ShouldReturnParsedStats()
    {
        // Arrange
        var csvContent = @"#,Name,MIN,PTS,FGM,FGA,FTM,FTA,2PA,2PM,TO,STL,BLK,PF,OREB,DREB,AST,REB,3PM,3PA
1,John Doe,30,25,10,15,5,6,8,6,3,2,1,4,2,5,7,7,3,5
2,Jane Smith,28,18,7,12,4,5,6,4,2,3,0,3,1,4,5,5,2,4";

        var mockFile = CreateMockFormFile(csvContent, "test.csv");

        // Act
        var result = await _csvParserService.ParseMatchStatsAsync(mockFile.Object);

        // Assert
        result.Should().HaveCount(2);
        
        var johnDoe = result.First(p => p.PlayerName == "John Doe");
        johnDoe.Minutes.Should().Be(30);
        johnDoe.Points.Should().Be(25);
        johnDoe.FieldGoalsMade.Should().Be(10);
        johnDoe.FieldGoalsAttempted.Should().Be(15);
        johnDoe.ThreePointersMade.Should().Be(3);
        johnDoe.ThreePointersAttempted.Should().Be(5);

        var janeSmith = result.First(p => p.PlayerName == "Jane Smith");
        janeSmith.Minutes.Should().Be(28);
        janeSmith.Points.Should().Be(18);
        janeSmith.Assists.Should().Be(5);
        janeSmith.Rebounds.Should().Be(5);
    }

//    [Fact]
//    public async Task ParseMatchStatsAsync_CsvWithTotalsRow_ShouldExcludeTotalsRow()
//    {
//        // Arrange
//        var csvContent = @"#,Name,MIN,PTS,FGM,FGA,FTM,FTA,2PA,2PM,TO,STL,BLK,PF,OREB,DREB,AST,REB,3PM,3PA
//1,John Doe,30,25,10,15,5,6,8,6,3,2,1,4,2,5,7,7,3,5
//Totals,58,43,17,27,9,11,14,10,5,5,1,7,3,9,12,12,5,9";

//        var mockFile = CreateMockFormFile(csvContent, "test.csv");

//        // Act
//        var result = await _csvParserService.ParseMatchStatsAsync(mockFile.Object);

//        // Assert
//        result.Should().HaveCount(1);
//        result.First().PlayerName.Should().Be("John Doe");
//    }

//    [Fact]
//    public async Task ParseMatchStatsAsync_CsvWithTeamRow_ShouldExcludeTeamRow()
//    {
//        // Arrange
//        var csvContent = @"#,Name,MIN,PTS,FGM,FGA,FTM,FTA,2PA,2PM,TO,STL,BLK,PF,OREB,DREB,AST,REB,3PM,3PA
//1,John Doe,30,25,10,15,5,6,8,6,3,2,1,4,2,5,7,7,3,5
// TEAM,58,43,17,27,9,11,14,10,5,5,1,7,3,9,12,12,5,9";

//        var mockFile = CreateMockFormFile(csvContent, "test.csv");

//        // Act
//        var result = await _csvParserService.ParseMatchStatsAsync(mockFile.Object);

//        // Assert
//        result.Should().HaveCount(1);
//        result.First().PlayerName.Should().Be("John Doe");
//    }

    [Fact]
    public async Task ParseMatchStatsAsync_CsvWithUnderscoreValues_ShouldConvertToZero()
    {
        // Arrange
        var csvContent = @"#,Name,MIN,PTS,FGM,FGA,FTM,FTA,2PA,2PM,TO,STL,BLK,PF,OREB,DREB,AST,REB,3PM,3PA
1,John Doe,30,25,_,15,_,6,8,6,3,2,1,4,2,5,7,7,_,5";

        var mockFile = CreateMockFormFile(csvContent, "test.csv");

        // Act
        var result = await _csvParserService.ParseMatchStatsAsync(mockFile.Object);

        // Assert
        result.Should().HaveCount(1);
        var player = result.First();
        player.FieldGoalsMade.Should().Be(0);
        player.FreeThrowsMade.Should().Be(0);
        player.ThreePointersMade.Should().Be(0);
    }

    [Fact]
    public async Task ParseMatchStatsAsync_EmptyPlayerName_ShouldSkipRow()
    {
        // Arrange
        var csvContent = @"#,Name,MIN,PTS,FGM,FGA,FTM,FTA,2PA,2PM,TO,STL,BLK,PF,OREB,DREB,AST,REB,3PM,3PA
1,John Doe,30,25,10,15,5,6,8,6,3,2,1,4,2,5,7,7,3,5
2,,28,18,7,12,4,5,6,4,2,3,0,3,1,4,5,5,2,4
3,Jane Smith,25,20,8,13,4,5,7,5,1,2,2,2,1,3,6,4,2,3";

        var mockFile = CreateMockFormFile(csvContent, "test.csv");

        // Act
        var result = await _csvParserService.ParseMatchStatsAsync(mockFile.Object);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.PlayerName == "John Doe");
        result.Should().Contain(p => p.PlayerName == "Jane Smith");
    }

    private Mock<IFormFile> CreateMockFormFile(string content, string fileName)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var mockFile = new Mock<IFormFile>();
        
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(stream.Length);
        mockFile.Setup(f => f.ContentType).Returns("text/csv");
        mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

        return mockFile;
    }
}