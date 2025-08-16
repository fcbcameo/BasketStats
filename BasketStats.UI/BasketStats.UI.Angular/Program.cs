using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

var app = builder.Build();

app.UseHttpsRedirection();

// Serve Angular static files
app.UseStaticFiles();
app.UseSpaStaticFiles();

// Expose the Blazor UI images directory to the Angular host under /images
var sharedImagesPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "BasketStats.UI", "wwwroot", "Images"));
if (Directory.Exists(sharedImagesPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(sharedImagesPath),
        RequestPath = "/images"
    });
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        // Proxy to Angular CLI dev server; VS will auto-start it via spa.proxy.json
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

app.Run();
