using Xunit;
using Moq;
using FluentAssertions;
using BasketStats.Application.Matches.Commands.DeleteMatch;
using BasketStats.Domain.Repositories;
using BasketStats.Domain;
using BasketStats.Domain.ValueObjects;
using Match = BasketStats.Domain.Match;

namespace BasketStats.Application.Tests.Matches.Commands;

public class DeleteMatchCommandHandlerTests
{
    private readonly Mock<IMatchRepository> _mockRepository;
    private readonly DeleteMatchCommandHandler _handler;

    public DeleteMatchCommandHandlerTests()
    {
        _mockRepository = new Mock<IMatchRepository>();
        _handler = new DeleteMatchCommandHandler(_mockRepository.Object);
    }

    // Replace all instances of 'DomainMatch' with 'Match' to match the type used in IMatchRepository signatures.

    [Fact]
    public async Task Handle_ExistingMatch_ShouldDeleteMatch()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var command = new DeleteMatchCommand(matchId);

        var existingMatch = Match.Create(Guid.NewGuid(), DateTime.Now, new List<PlayerStats>());

        _mockRepository.Setup(x => x.GetByIdAsync(matchId))
                      .ReturnsAsync(existingMatch);
        _mockRepository.Setup(x => x.DeleteAsync(existingMatch))
                      .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(x => x.GetByIdAsync(matchId), Times.Once);
        _mockRepository.Verify(x => x.DeleteAsync(existingMatch), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingMatch_ShouldNotCallDelete()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var command = new DeleteMatchCommand(matchId);

        _mockRepository.Setup(x => x.GetByIdAsync(matchId))
                      .ReturnsAsync((Match)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(x => x.GetByIdAsync(matchId), Times.Once);
        _mockRepository.Verify(x => x.DeleteAsync(It.IsAny<Match>()), Times.Never);
    }
}