// src/BasketStats.Infrastructure/Services/CsvConverters/UnderscoreToZeroIntConverter.cs
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BasketStats.Infrastructure.Services.CsvConverters;

public class UnderscoreToZeroIntConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (text == "-" || text == "_")
        {
            return 0;
        }

        if (int.TryParse(text, out var result))
        {
            return result;
        }

        throw new TypeConverterException(this, memberMapData, text, row.Context, $"Cannot convert '{text}' to an integer.");
    }
}