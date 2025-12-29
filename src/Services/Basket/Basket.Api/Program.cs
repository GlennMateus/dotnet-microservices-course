
using Basket.Api.Configuration;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddApplicationServices();

// Data services
builder.Services.AddDataServices(builder.Configuration);

// Grpc Services
builder.Services.AddGrpcServices(builder.Configuration);

// Cross cutting services
builder.Services.AddCrossCuttingServices(builder.Configuration);

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
