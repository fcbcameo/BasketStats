using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BasketStats.Infrastructure.Persistence;
using BasketStats.Infrastructure.Persistence.Repositories;
using BasketStats.Domain;

namespace BasketStats.Infrastructure.Tests.Repositories;

public class EfCompetitionRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly EfCompetitionRepository _repository;

    public EfCompetitionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new EfCompetitionRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ValidCompetition_ShouldAddToDatabase()
    {
        // Arrange
        var competition = Competition.Create("Test Competition");

        // Act
        await _repository.AddAsync(competition);

        // Assert
        var savedCompetition = await _context.Competitions.FirstOrDefaultAsync(c => c.Id == competition.Id);
        savedCompetition.Should().NotBeNull();
        savedCompetition.Name.Should().Be("Test Competition");
    }

    [Fact]
    public async Task GetAllAsync_WithCompetitions_ShouldReturnAllCompetitions()
    {
        // Arrange
        var competition1 = Competition.Create("Competition 1");
        var competition2 = Competition.Create("Competition 2");
        
        await _repository.AddAsync(competition1);
        await _repository.AddAsync(competition2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Competition 1");
        result.Should().Contain(c => c.Name == "Competition 2");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingCompetition_ShouldReturnCompetition()
    {
        // Arrange
        var competition = Competition.Create("Test Competition");
        await _repository.AddAsync(competition);

        // Act
        var result = await _repository.GetByIdAsync(competition.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Competition");
        result.Id.Should().Be(competition.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingCompetition_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}