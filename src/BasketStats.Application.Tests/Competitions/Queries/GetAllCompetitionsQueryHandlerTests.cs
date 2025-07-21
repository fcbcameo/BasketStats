using Xunit;
using Moq;
using FluentAssertions;
using BasketStats.Application.Competitions.Queries.GetAllCompetitions;
using BasketStats.Domain.Repositories;
using BasketStats.Domain;

namespace BasketStats.Application.Tests.Competitions.Queries;

public class GetAllCompetitionsQueryHandlerTests
{
    private readonly Mock<ICompetitionRepository> _mockRepository;
    private readonly GetAllCompetitionsQueryHandler _handler;

    public GetAllCompetitionsQueryHandlerTests()
    {
        _mockRepository = new Mock<ICompetitionRepository>();
        _handler = new GetAllCompetitionsQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WithCompetitions_ShouldReturnCompetitionDtos()
    {
        // Arrange
        var competitions = new List<Competition>
        {
            Competition.Create("Basketball League 2024"),
            Competition.Create("Summer Tournament"),
            Competition.Create("Championship Series")
        };

        _mockRepository.Setup(x => x.GetAllAsync())
                      .ReturnsAsync(competitions);

        var query = new GetAllCompetitionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(dto => dto.Name == "Basketball League 2024");
        result.Should().Contain(dto => dto.Name == "Summer Tournament");
        result.Should().Contain(dto => dto.Name == "Championship Series");
        
        // Verify all DTOs have valid IDs
        result.Should().AllSatisfy(dto => dto.Id.Should().NotBe(Guid.Empty));
    }

    [Fact]
    public async Task Handle_WithNoCompetitions_ShouldReturnEmptyList()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetAllAsync())
                      .ReturnsAsync(new List<Competition>());

        var query = new GetAllCompetitionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var competition = Competition.Create("Test Competition");
        var competitions = new List<Competition> { competition };

        _mockRepository.Setup(x => x.GetAllAsync())
                      .ReturnsAsync(competitions);

        var query = new GetAllCompetitionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var dto = result.Single();
        dto.Id.Should().Be(competition.Id);
        dto.Name.Should().Be(competition.Name);
    }
}