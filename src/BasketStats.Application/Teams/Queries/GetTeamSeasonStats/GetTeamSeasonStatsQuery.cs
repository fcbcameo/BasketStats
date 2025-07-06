// src/BasketStats.Application/Teams/Queries/GetTeamSeasonStats/GetTeamSeasonStatsQuery.cs
using BasketStats.Application.DTOs;
using MediatR;

namespace BasketStats.Application.Teams.Queries.GetTeamSeasonStats;

public record GetTeamSeasonStatsQuery(Guid? CompetitionId = null) : IRequest<TeamSeasonStatsDto>;