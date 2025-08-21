using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace BasketStats.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Resolve appsettings.json from common locations to avoid null configuration during EF tooling
        var cwd = Directory.GetCurrentDirectory();
        var candidates = new[]
        {
            Path.Combine(cwd, "appsettings.json"),
            Path.Combine(cwd, "..", "BasketStats.Web", "appsettings.json"),
            Path.Combine(cwd, "..", "..", "src", "BasketStats.Web", "appsettings.json"),
            Path.Combine(AppContext.BaseDirectory, "appsettings.json")
        };

        string? configDir = candidates
            .Select(Path.GetDirectoryName)
            .FirstOrDefault(dir => dir != null && File.Exists(Path.Combine(dir!, "appsettings.json")));

        var cfgBuilder = new ConfigurationBuilder();
        if (configDir != null)
        {
            cfgBuilder.SetBasePath(configDir)
                      .AddJsonFile("appsettings.json", optional: true)
                      .AddJsonFile("appsettings.Development.json", optional: true);
        }
        cfgBuilder.AddEnvironmentVariables();

        var configuration = cfgBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var conn = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(conn))
        {
            // Fallback for design-time operations
            conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BasketStats;Integrated Security=True;TrustServerCertificate=True";
        }
        optionsBuilder.UseSqlServer(conn);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
