// src/BasketStats.Application/Players/Queries/GetPlayerSeasonStats/GetPlayerSeasonStatsQuery.cs
using BasketStats.Application.DTOs;
using MediatR;

namespace BasketStats.Application.Players.Queries.GetPlayerSeasonStats;

public record GetPlayerSeasonStatsQuery(Guid PlayerId, Guid? CompetitionId = null) : IRequest<PlayerSeasonStatsDto?>;