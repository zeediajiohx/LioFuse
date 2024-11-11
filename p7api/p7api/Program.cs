using Microsoft.EntityFrameworkCore;
using pseven.Models.well;
using Grpc.AspNetCore.ClientFactory;
using GrpcGreeterClient.Wells;
using GrpcGreeterClient.Structures;
using GrpcGreeterClient.Services;
using Microsoft.OpenApi.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddDbContext<WELL_liteContext>(opt =>
//    opt.UseInMemoryDatabase("WELL_LiteList")
//    );
//builder.Services.AddDbContext<WellContext>(opt =>
//        opt.UseInMemoryDatabase("WellList"));
builder.Services.AddDbContext<WellContext>(opt =>
    opt.UseInMemoryDatabase("wellList"));

// Register WELL_liteContext
builder.Services.AddDbContext<WELL_liteContext>(opt =>
    opt.UseInMemoryDatabase("WELL_LiteList"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });

    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddGrpc();
//builder.Services.AddGrpcServiceConfiguration(builder.Configuration);
builder.Services.AddSingleton(services =>
{
    var channel = GrpcChannel.ForAddress("https://localhost:7004"); 
    return new WellService.WellServiceClient(channel);
});
builder.Services.AddSingleton(services =>
{
	var channel = GrpcChannel.ForAddress("https://localhost:7004");
	return new StructureService.StructureServiceClient(channel);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
});
//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
//app.MapGrpcService<Grpc_well_lite>();

app.Run();
//public static class GrpcServiceExtenstons
//{
//    public static void AddGrpcServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddGrpcClient<WellService.WellServiceClient>(options =>
//        {
//            options.Address = new Uri(configuration.GetValue<string>("GrpcUrls: WellLIte"));
//        });
//    }
//}
