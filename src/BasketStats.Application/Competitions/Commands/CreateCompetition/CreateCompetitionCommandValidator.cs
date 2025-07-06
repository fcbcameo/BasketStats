// src/BasketStats.Application/Competitions/Commands/CreateCompetition/CreateCompetitionCommandValidator.cs
using FluentValidation;

namespace BasketStats.Application.Competitions.Commands.CreateCompetition;

public class CreateCompetitionCommandValidator : AbstractValidator<CreateCompetitionCommand>
{
    public CreateCompetitionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Competition name cannot be empty.")
            .MaximumLength(100).WithMessage("Competition name must not exceed 100 characters.");
    }
}