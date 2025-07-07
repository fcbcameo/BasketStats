// in src/BasketStats.Application/DTOs/PlayerDto.cs
namespace BasketStats.Application.DTOs;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}