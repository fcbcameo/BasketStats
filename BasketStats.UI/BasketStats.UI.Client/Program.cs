using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Http; // Add this namespace to resolve 'AddHttpClient'

var builder = WebAssemblyHostBuilder.CreateDefault(args);


await builder.Build().RunAsync();
