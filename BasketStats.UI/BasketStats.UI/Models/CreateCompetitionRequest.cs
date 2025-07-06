// In src/BasketStats.UI/Models/CreateCompetitionRequest.cs
using System.ComponentModel.DataAnnotations;

namespace BasketStats.UI.Models;

public class CreateCompetitionRequest
{
    [Required(ErrorMessage = "Competition name is required.")]
    public string Name { get; set; } = string.Empty;
}