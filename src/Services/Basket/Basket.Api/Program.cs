using Basket.Api.Configuration;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services
    .AddApplicationServices()
// Data services
    .AddDataServices(builder.Configuration)
// Grpc Services
    .AddGrpcServices(builder.Configuration)
// Cross cutting services
    .AddCrossCuttingServices(builder.Configuration);

var app = builder.Build();

// HTTP pipeline
app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
