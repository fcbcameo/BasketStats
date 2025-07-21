using Xunit;
using Moq;
using FluentAssertions;
using BasketStats.Application.Players.Queries.GetPlayerSeasonStats;
using BasketStats.Domain.Repositories;
using BasketStats.Domain;
using BasketStats.Domain.ValueObjects;

namespace BasketStats.Application.Tests.Players.Queries;

public class GetPlayerSeasonStatsQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _mockPlayerRepository;
    private readonly Mock<IMatchRepository> _mockMatchRepository;
    private readonly GetPlayerSeasonStatsQueryHandler _handler;

    public GetPlayerSeasonStatsQueryHandlerTests()
    {
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockMatchRepository = new Mock<IMatchRepository>();
        _handler = new GetPlayerSeasonStatsQueryHandler((IMatchRepository)_mockPlayerRepository.Object, (IPlayerRepository)_mockMatchRepository.Object);
    }

   // [Fact]
    //public async Task Handle_PlayerNotFound_ReturnsNull()
    //{
    //    // Arrange
    //    var playerId = Guid.NewGuid();
    //    var query = new GetPlayerSeasonStatsQuery(playerId, null);

    //    _mockPlayerRepository.Setup(x => x.GetByIdAsync(playerId))
    //                       .ReturnsAsync((Player)null);

    //    // Act
    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.Should().BeNull();
    //}

    //[Fact]
    //public async Task Handle_PlayerFoundButNoMatches_ReturnsEmptyStats()
    //{
    //    // Arrange
    //    var playerId = Guid.NewGuid();
    //    var player = Player.Create("Test Player");
    //    var query = new GetPlayerSeasonStatsQuery(playerId, null);

    //    _mockPlayerRepository.Setup(x => x.GetByIdAsync(playerId))
    //                       .ReturnsAsync(player);

    //    // Fix ambiguous reference and use ReturnsAsync for Task<IEnumerable<Match>>
    //    _mockMatchRepository.Setup(x => x.GetAllAsync())
    //                      .ReturnsAsync(Enumerable.Empty<Domain.Match>());

    //    // Act
    //    var result = await _handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.PlayerName.Should().Be("Test Player");
    //    result.TotalPoints.Should().Be(0);
    //    result.TotalMinutes.Should().Be(0);
    //}
}