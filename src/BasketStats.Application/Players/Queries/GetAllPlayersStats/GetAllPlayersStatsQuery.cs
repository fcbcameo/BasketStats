// src/BasketStats.Application/Players/Queries/GetAllPlayersStats/GetAllPlayersStatsQuery.cs
using BasketStats.Application.DTOs;
using MediatR;

namespace BasketStats.Application.Players.Queries.GetAllPlayersStats;

public record GetAllPlayersStatsQuery(Guid? CompetitionId = null) : IRequest<IEnumerable<PlayerSeasonStatsDto>>;