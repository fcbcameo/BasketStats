using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BasketStats.Infrastructure.Persistence;
using BasketStats.Infrastructure.Persistence.Repositories;
using BasketStats.Domain;

namespace BasketStats.Infrastructure.Tests.Repositories;

public class EfPlayerRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly EfPlayerRepository _repository;

    public EfPlayerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new EfPlayerRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ValidPlayer_ShouldAddToDatabase()
    {
        // Arrange
        var player = Player.Create("John Doe");

        // Act
        await _repository.AddAsync(player);

        // Assert
        var savedPlayer = await _context.Players.FirstOrDefaultAsync(p => p.Id == player.Id);
        savedPlayer.Should().NotBeNull();
        savedPlayer.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetAllAsync_WithPlayers_ShouldReturnAllPlayers()
    {
        // Arrange
        var player1 = Player.Create("Player 1");
        var player2 = Player.Create("Player 2");
        
        await _repository.AddAsync(player1);
        await _repository.AddAsync(player2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Player 1");
        result.Should().Contain(p => p.Name == "Player 2");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingPlayer_ShouldReturnPlayer()
    {
        // Arrange
        var player = Player.Create("Test Player");
        await _repository.AddAsync(player);

        // Act
        var result = await _repository.GetByIdAsync(player.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Player");
        result.Id.Should().Be(player.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingPlayer_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_ExistingPlayer_ShouldReturnPlayer()
    {
        // Arrange
        var player = Player.Create("Unique Player Name");
        await _repository.AddAsync(player);

        // Act
        var result = await _repository.GetByNameAsync("Unique Player Name");

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Unique Player Name");
    }

    [Fact]
    public async Task GetByNameAsync_NonExistingPlayer_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByNameAsync("Non Existing Player");

        // Assert
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}