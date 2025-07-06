// src/BasketStats.Infrastructure/Services/CsvParserService.cs
using BasketStats.Application.DTOs;
using BasketStats.Application.Services;
using CsvHelper;
using CsvHelper.Configuration;
using BasketStats.Infrastructure.Services.CsvConverters; 
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BasketStats.Infrastructure.Services;

public class CsvParserService : ICsvParser
{
    public async Task<IEnumerable<ParsedPlayerStat>> ParseMatchStatsAsync(IFormFile file)
    {
        var records = new List<ParsedPlayerStat>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // TODO: Your CSV might use a different delimiter
            Delimiter = ",",
        };

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, config);

        // *** REGISTER THE CUSTOM CONVERTER HERE ***
        csv.Context.TypeConverterCache.AddConverter<int>(new UnderscoreToZeroIntConverter());


        // 1. Skip rows until we find the header row, which starts with "#"
        while (await csv.ReadAsync())
        {
            var firstField = csv.GetField(0);
            if (firstField?.Trim() == "#")
            {
                // This is the header row. CsvReader is now positioned correctly.
                csv.ReadHeader();
                break;
            }
        }

        // 2. Read the player stat records
        while (await csv.ReadAsync())
        {
            var playerName = csv.GetField("Name");

            // 3. Stop if we hit the "Totals" row
            if ((string.Equals(playerName, "Totals", StringComparison.OrdinalIgnoreCase)) || (string.Equals(playerName, " TEAM", StringComparison.OrdinalIgnoreCase)))
            {
                break;
            }

            // Ensure player name is not empty
            if (string.IsNullOrWhiteSpace(playerName)) continue;

            var record = new ParsedPlayerStat
            {
                PlayerName = playerName,
                Minutes = csv.GetField<int>("MIN"),
                Points = csv.GetField<int>("PTS"),
                FieldGoalsMade = csv.GetField<int>("FGM"),
                FieldGoalsAttempted = csv.GetField<int>("FGA"),
                FreeThrowsMade = csv.GetField<int>("FTM"),
                FreeThrowsAttempted = csv.GetField<int>("FTA"),
                TwoPointersAttempted = csv.GetField<int>("2PA"),
                TwoPointersMade = csv.GetField<int>("2PM"),
                Turnovers = csv.GetField<int>("TO"),
                Steals = csv.GetField<int>("STL"),
                Blocks = csv.GetField<int>("BLK"),
                PersonalFouls = csv.GetField<int>("PF"),
                OffensiveRebounds = csv.GetField<int>("OREB"),
                DefensiveRebounds = csv.GetField<int>("DREB"),
                Assists = csv.GetField<int>("AST"),
                Rebounds = csv.GetField<int>("REB"),
                ThreePointersMade = csv.GetField<int>("3PM"),
                ThreePointersAttempted = csv.GetField<int>("3PA")
                
            };
            records.Add(record);
        }

        return records;
    }
}