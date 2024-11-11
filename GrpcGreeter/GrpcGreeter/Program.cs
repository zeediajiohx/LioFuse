
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrpcGreeter.Services;
using System.Net.Http.Headers;
using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using GrpcGreeter.serizaliza;
using Grpc.Net.Client;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddHttpClient("WellFetchApiClient", client =>
{
    client.BaseAddress = new Uri("https://tj06.evt.slb.com/msd/"); 
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6Ik1UY3dNemN5TVRZd01nPT0iLCJ0eXAiOiJKV1QifQ.eyJzdWJpZCI6Ilk4OUhvcU01TXFtY2dzZTl1N0xIRFBQMzVTUVViTjlfOVpSUEtac0E5aEkiLCJlbWFpbCI6Imp6aGFuZzIwMkBzbGIuY29tIiwiY29tbW9ubmFtZSI6Ikppbmh1aSBaSEFORyIsImZpcnN0bmFtZSI6Ikppbmh1aSIsImxhc3RuYW1lIjoiWkhBTkciLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy9jbGFpbXMvQWNjb3VudElkIjoiRHJpbGxQbGFuU2hhcmVkIiwiY3RpZCI6IkRyaWxsUGxhbi1TaGFyZWQiLCJkcG4iOiJEcmlsbFBsYW4tU2hhcmVkIiwiZnRyIjpbIlN0YW5kYXJkX0FjY2VzcyIsIk1hbmFnZV9NU0QiLCJNbmdfQ29ycF9TZXR0aW5ncyJdLCJhdXRoX3RpbWUiOiIxMS8wNi8yMDI0IDAxOjEzOjMwIiwiZHBpZCI6ImRyaWxscGxhbi1zaGFyZWQtZHJpbGxwbGFuLXNoYSIsImVudGl0bGVtZW50cyI6WyJtc2QiLCJjb3Jwb3JhdGVfc2V0dGluZ3MiXSwibmFtZWlkIjoianpoYW5nMjAyQHNsYi5jb20iLCJuYmYiOjE3MzA4NTk0NjYsImV4cCI6MTczMDg2MTI2NiwiaWF0IjoxNzMwODU5NDY2LCJpc3MiOiJ0YWlqaS1zdHMtZGV2LmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjoiTG9jYWxob3N0VG9rZW5DbGllbnQifQ.tbYLS1REWIjFwd-3TnxGLFJtp2RZM3Krg7l6lohmr56KZ58H6W0eXAkqixvHwisEwQqqZqEcFLLY00XkVZxBoP4P6KCw4oX2VJ6AuWDBBKnW2ypcQLHi6gdBMHVvEhOY1d0qQcu4x9QYYd6MFIML-aGReYefvMGh2eFjNwDhmtOul_kydrRXTMeSapES6JaBrSwFjaXI6tqC6tVuv2uAa89FGS-EICgras9bmjIqdUpxWSXxKNT4UgQRajrOGKg2Fk7Y64XngN0qXudbOFVrGETjYJ9uWVkXYV5XlUmpuiD8_YnOKSodnmIAzay208OsFsRBXJqwcRFsqPStQFfknQ"); 
});
//builder.Services.AddHttpClient("StructureFetchApiClient", client =>
//{
//    client.BaseAddress = new Uri("https://tj06.evt.slb.com/msd/");
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
//});
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters =
    {
        new NullToEmptyStringConverter()
    }
};
builder.Services.AddSingleton(jsonOptions);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// Configure the HTTP request pipeline.
app.MapGrpcService<WellService>();
app.MapGrpcService<StructureService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


