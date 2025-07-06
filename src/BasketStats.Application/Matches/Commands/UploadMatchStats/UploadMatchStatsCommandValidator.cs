using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BasketStats.Application.Matches.Commands.UploadMatchStats;

public class UploadMatchStatsCommandValidator : AbstractValidator<UploadMatchStatsCommand>
{
    public UploadMatchStatsCommandValidator()
    {
        RuleFor(x => x.CompetitionId)
            .NotEmpty().WithMessage("CompetitionId is required.");
        RuleFor(x => x.CsvFile)
            .NotNull().WithMessage("CSV file is required.")
            .Must(f => f != null && f.Length > 0).WithMessage("CSV file must not be empty.")
            .Must(f => f != null && f.ContentType == "text/csv" || f.ContentType == "application/vnd.ms-excel")
            .WithMessage("File must be a CSV.");
    }
}
