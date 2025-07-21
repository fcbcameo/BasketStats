using Xunit;
using Moq;
using FluentAssertions;
using BasketStats.Application.Competitions.Commands.CreateCompetition;
using BasketStats.Domain;
using BasketStats.Domain.Repositories;

namespace BasketStats.Application.Tests.Competitions.Commands;

public class CreateCompetitionCommandHandlerTests
{
    private readonly Mock<ICompetitionRepository> _mockRepository;
    private readonly CreateCompetitionCommandHandler _handler;

    public CreateCompetitionCommandHandlerTests()
    {
        _mockRepository = new Mock<ICompetitionRepository>();
        _handler = new CreateCompetitionCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsCompetitionId()
    {
        // Arrange
        var command = new CreateCompetitionCommand("Test Competition");
        var competitionId = Guid.NewGuid();
        
        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Competition>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _mockRepository.Verify(x => x.AddAsync(It.Is<Competition>(c => c.Name == "Test Competition")), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesCompetitionWithCorrectName()
    {
        // Arrange
        var command = new CreateCompetitionCommand("Basketball League 2024");
        Competition capturedCompetition = null;
        
        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Competition>()))
                      .Callback<Competition>(c => capturedCompetition = c)
                      .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedCompetition.Should().NotBeNull();
        capturedCompetition.Name.Should().Be("Basketball League 2024");
        capturedCompetition.Id.Should().NotBe(Guid.Empty);
    }
}