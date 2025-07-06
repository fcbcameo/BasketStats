// src/BasketStats.Infrastructure/Services/CsvParserService.cs
using BasketStats.Application.Services;
using BasketStats.Domain.ValueObjects;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BasketStats.Infrastructure.Services;

public class CsvParserService : ICsvParser
{
    // This is a temporary, simplified mapping.
    // We'll need to make this more robust to handle your exact CSV format.
    public async Task<IEnumerable<PlayerStats>> ParseMatchStatsAsync(IFormFile file)
    {
        var records = new List<PlayerStats>();
        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // This assumes the CSV has headers that match our target properties.
        // Based on your files, we need to skip initial rows and handle the specific structure.
        // For now, let's assume a simplified CSV for demonstration.
        // Example Headers: PlayerId, Points, Assists, Rebounds, etc.
        await csv.ReadAsync();
        csv.ReadHeader();
        while (await csv.ReadAsync())
        {
            var record = new PlayerStats
            {
                // This mapping is VERY IMPORTANT and must match your CSV columns.
                // For now, we are just creating dummy data from the name.
                // In the next step, we will make this real.
                PlayerId = Guid.NewGuid(), // Placeholder
                Points = csv.GetField<int>("PTS"),
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