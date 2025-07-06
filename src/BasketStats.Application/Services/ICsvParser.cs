// src/BasketStats.Application/Services/ICsvParser.cs
using BasketStats.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace BasketStats.Application.Services;

public interface ICsvParser
{
    //TODO Refactor to use a Stream instead of IFormFile!
    Task<IEnumerable<ParsedPlayerStat>> ParseMatchStatsAsync(IFormFile file);
}