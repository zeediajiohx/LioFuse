using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorFronted;
using System.Net.Http;
using BlazorFronted.Services;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using MatBlazor;
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7170/") });
builder.Services.AddScoped<WellService>();
builder.Services.AddScoped<StructureService>();
builder.Services.AddMatBlazor();

await builder.Build().RunAsync();

















