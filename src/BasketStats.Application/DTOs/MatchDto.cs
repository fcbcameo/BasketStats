// in src/BasketStats.Application/DTOs/MatchDto.cs
public class MatchDto
{
    public Guid Id { get; set; }
    public DateTime MatchDate { get; set; }
    public Guid CompetitionId { get; set; }
    public int PlayerStatCount { get; set; }
}