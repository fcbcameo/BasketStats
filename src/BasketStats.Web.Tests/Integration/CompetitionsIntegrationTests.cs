using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using BasketStats.Application.DTOs;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using BasketStats.Application.Competitions.Queries.GetAllCompetitions;

namespace BasketStats.Web.Tests.Integration;

public class CompetitionsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CompetitionsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    //[Fact]
    //public async Task GetCompetitions_ShouldReturnEmptyList_WhenNoCompetitions()
    //{
    //    // Act
    //    var response = await _client.GetAsync("/api/competitions");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //    var competitions = await response.Content.ReadFromJsonAsync<List<CompetitionDto>>();
    //    competitions.Should().NotBeNull();
    //    competitions.Should().BeEmpty();
    //}

    //[Fact]
    //public async Task CreateCompetition_ShouldReturnCreated_WhenValidRequest()
    //{
    //    // Arrange
    //    var request = new { Name = "Test Competition" };

    //    // Act
    //    var response = await _client.PostAsJsonAsync("/api/competitions", request);

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.Created);
    //    var responseContent = await response.Content.ReadAsStringAsync();
    //    responseContent.Should().Contain("Test Competition");
    //}

    //[Fact]
    //public async Task CreateCompetition_ShouldReturnBadRequest_WhenInvalidRequest()
    //{
    //    // Arrange
    //    var request = new { Name = "" }; // Invalid empty name

    //    // Act
    //    var response = await _client.PostAsJsonAsync("/api/competitions", request);

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //}

    //[Fact]
    //public async Task GetCompetitions_ShouldReturnCompetitions_AfterCreating()
    //{
    //    // Arrange
    //    var request1 = new { Name = "Competition 1" };
    //    var request2 = new { Name = "Competition 2" };

    //    // Act
    //    await _client.PostAsJsonAsync("/api/competitions", request1);
    //    await _client.PostAsJsonAsync("/api/competitions", request2);
        
    //    var response = await _client.GetAsync("/api/competitions");

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //    var competitions = await response.Content.ReadFromJsonAsync<List<CompetitionDto>>();
    //    competitions.Should().HaveCount(2);
    //    competitions.Should().Contain(c => c.Name == "Competition 1");
    //    competitions.Should().Contain(c => c.Name == "Competition 2");
    //}
}