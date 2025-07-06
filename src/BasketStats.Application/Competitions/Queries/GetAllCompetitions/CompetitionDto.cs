// src/BasketStats.Application/Competitions/Queries/GetAllCompetitions/CompetitionDto.cs
namespace BasketStats.Application.Competitions.Queries.GetAllCompetitions;

public class CompetitionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}