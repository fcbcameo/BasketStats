using BasketStats.UI.Components;
using BasketStats.UI.Services;
using Radzen;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add Radzen services
builder.Services.AddRadzenComponents();

// Add NotificationService for Radzen notifications
builder.Services.AddScoped<NotificationService>();

// Add PDF Report Service
builder.Services.AddScoped<PdfReportService>();

// Add the HttpClient configuration  
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7119");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    // This is good practice for development to avoid SSL issues
    // if you don't have a trusted local certificate.
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});

// We need to configure the JSON options for Blazor's GetFromJsonAsync, etc.
// This is not directly available on HttpClient, so we configure it for the app.
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    // This ensures that property names are not changed during serialization.
    options.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BasketStats.UI.Client._Imports).Assembly);

app.Run();
