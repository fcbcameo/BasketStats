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
        try
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
            var headerFound = false;
            while (await csv.ReadAsync())
            {
                var firstField = csv.GetField(0);
                if (firstField?.Trim() == "#")
                {
                    // This is the header row. CsvReader is now positioned correctly.
                    csv.ReadHeader();
                    headerFound = true;
                    break;
                }
            }

            if (!headerFound)
            {
                throw new InvalidOperationException("CSV header row not found. Expected a header row starting with '#' as the first field.");
            }

            var headerRecord = csv.HeaderRecord;
            if (headerRecord == null || headerRecord.Length == 0)
            {
                throw new InvalidOperationException("CSV header could not be read or is empty.");
            }

            // Required fields we expect in the CSV
            var requiredFields = new[] { "Name", "MIN", "PTS", "FGM", "FGA", "FTM", "FTA", "2PA", "2PM", "TO", "STL", "BLK", "PF", "OREB", "DREB", "AST", "REB", "3PM", "3PA" };
            var missing = requiredFields.Where(f => !headerRecord.Any(h => string.Equals(h, f, StringComparison.OrdinalIgnoreCase))).ToList();
            if (missing.Any())
            {
                throw new InvalidOperationException($"CSV is missing required columns: {string.Join(", ", missing)}");
            }

            // 2. Read the player stat records
            while (await csv.ReadAsync())
            {
                string playerName;
                try
                {
                    playerName = csv.GetField("Name");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to read 'Name' field: {ex.Message}", ex);
                }

                // 3. Stop if we hit the "Totals" row
                if ((string.Equals(playerName, "Totals", StringComparison.OrdinalIgnoreCase)) || (string.Equals(playerName, " TEAM", StringComparison.OrdinalIgnoreCase)))
                {
                    break;
                }

                // Ensure player name is not empty
                if (string.IsNullOrWhiteSpace(playerName)) continue;

                try
                {
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
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to parse player stat for player '{playerName}': {ex.Message}", ex);
                }
            }

            return records;
        }
        catch (Exception ex)
        {
            // Wrap and rethrow with clearer context for the caller
            throw new InvalidOperationException($"Failed to parse CSV file: {ex.Message}", ex);
        }
    }
}