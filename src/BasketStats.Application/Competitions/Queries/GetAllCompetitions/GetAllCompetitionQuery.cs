// src/BasketStats.Application/Competitions/Queries/GetAllCompetitions/GetAllCompetitionsQuery.cs
using MediatR;

namespace BasketStats.Application.Competitions.Queries.GetAllCompetitions;

public record GetAllCompetitionsQuery() : IRequest<IEnumerable<CompetitionDto>>;