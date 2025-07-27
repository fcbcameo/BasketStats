using BasketStats.Application.DTOs;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace BasketStats.UI.Services;

public class PdfReportService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PdfReportService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public byte[] GeneratePlayerStatsReport(IEnumerable<PlayerSeasonStatsDto> playersStats, string competitionName)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4.Rotate(), 36, 36, 72, 36); // Landscape orientation
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // NBA Colors
        var nbaBlue = new BaseColor(29, 66, 138);
        var nbaRed = new BaseColor(201, 8, 42);
        var nbaOrange = new BaseColor(245, 132, 38);
        var grayColor = new BaseColor(128, 128, 128);
        var whiteColor = new BaseColor(255, 255, 255);
        var blackColor = new BaseColor(0, 0, 0);

        // Add logo if exists
        try
        {
            var logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "basketballstats.png");
            if (File.Exists(logoPath))
            {
                var logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(60f, 60f);
                logo.Alignment = Element.ALIGN_LEFT;
                
                var logoTable = new PdfPTable(2);
                logoTable.WidthPercentage = 100;
                logoTable.SetWidths(new float[] { 1, 4 });
                
                var logoCell = new PdfPCell(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                logoTable.AddCell(logoCell);
                
                var titleCell = new PdfPCell(new Phrase("BasketStats - Player Statistics Report", 
                    new Font(Font.HELVETICA, 18, Font.BOLD, nbaBlue)));
                titleCell.Border = Rectangle.NO_BORDER;
                titleCell.HorizontalAlignment = Element.ALIGN_LEFT;
                titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                logoTable.AddCell(titleCell);
                
                document.Add(logoTable);
                document.Add(new Paragraph(" ")); // Space
            }
        }
        catch
        {
            // If logo fails to load, just add the title
            var title = new Paragraph("BasketStats - Player Statistics Report", 
                new Font(Font.HELVETICA, 18, Font.BOLD, nbaBlue));
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph(" "));
        }

        // Add report info
        var reportInfo = new Paragraph($"Competition: {competitionName}", 
            new Font(Font.HELVETICA, 12, Font.BOLD, nbaRed));
        reportInfo.Alignment = Element.ALIGN_CENTER;
        document.Add(reportInfo);

        var generatedInfo = new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", 
            new Font(Font.HELVETICA, 10, Font.NORMAL, grayColor));
        generatedInfo.Alignment = Element.ALIGN_CENTER;
        document.Add(generatedInfo);

        document.Add(new Paragraph(" ")); // Space

        // Create the statistics table
        var table = new PdfPTable(17); // Number of columns
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 3f, 1f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.5f, 1.5f, 1.5f, 1.5f });

        // Header style
        var headerFont = new Font(Font.HELVETICA, 8, Font.BOLD, whiteColor);
        
        // Add headers
        var headers = new[]
        {
            "Player", "GP", "PPG", "RPG", "APG", "FG%", "3P%", "FT%", "SPG", "BPG", "TOV", "MPG", "EFF", "Total PTS", "Total REB", "Total AST", "Total STL"
        };

        foreach (var header in headers)
        {
            var headerCell = new PdfPCell(new Phrase(header, headerFont));
            headerCell.BackgroundColor = nbaBlue;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell.Padding = 4;
            table.AddCell(headerCell);
        }

        // Data style
        var dataFont = new Font(Font.HELVETICA, 7, Font.NORMAL, blackColor);
        var nameFont = new Font(Font.HELVETICA, 7, Font.BOLD, nbaOrange);

        // Add data rows
        foreach (var player in playersStats.OrderByDescending(p => p.PointsPerGame))
        {
            // Player name
            var nameCell = new PdfPCell(new Phrase(player.PlayerName, nameFont));
            nameCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nameCell.Padding = 3;
            table.AddCell(nameCell);

            // Data cells
            var values = new[]
            {
                player.GamesPlayed.ToString(),
                player.PointsPerGame.ToString("F1"),
                player.ReboundsPerGame.ToString("F1"),
                player.AssistsPerGame.ToString("F1"),
                player.FieldGoalPercentage.ToString("F1"),
                player.ThreePointPercentage.ToString("F1"),
                player.FreeThrowPercentage.ToString("F1"),
                player.StealsPerGame.ToString("F1"),
                player.BlocksPerGame.ToString("F1"),
                player.TurnoversPerGame.ToString("F1"),
                player.MinutesPerGame.ToString("F1"),
                player.Efficiency.ToString("F1"),
                player.TotalPoints.ToString(),
                player.TotalRebounds.ToString(),
                player.TotalAssists.ToString(),
                player.TotalSteals.ToString()
            };

            foreach (var value in values)
            {
                var dataCell = new PdfPCell(new Phrase(value, dataFont));
                dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                dataCell.Padding = 3;
                table.AddCell(dataCell);
            }
        }

        document.Add(table);

        // Add footer
        document.Add(new Paragraph(" "));
        var footer = new Paragraph("This report contains comprehensive player statistics across selected competitions.", 
            new Font(Font.HELVETICA, 8, Font.ITALIC, grayColor));
        footer.Alignment = Element.ALIGN_CENTER;
        document.Add(footer);

        document.Close();
        writer.Close();

        return memoryStream.ToArray();
    }
}