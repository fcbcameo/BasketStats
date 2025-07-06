// src/BasketStats.Application/Matches/Commands/UploadMatchStats/UploadMatchStatsCommand.cs
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BasketStats.Application.Matches.Commands.UploadMatchStats;

public record UploadMatchStatsCommand(Guid CompetitionId, IFormFile CsvFile) : IRequest<Guid>;