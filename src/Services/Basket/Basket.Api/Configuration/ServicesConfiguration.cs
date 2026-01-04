using Discount.Grpc;

namespace Basket.Api.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var programAssembly = typeof(Program).Assembly;
        services.AddCarter();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(programAssembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMarten(options =>
        {
            options.Connection(configuration.GetConnectionString("Database")!);
            options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
        }).UseLightweightSessions();

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis")!;
        });

        return services;
    }

    public static IServiceCollection AddCrossCuttingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);

        return services;
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return handler;
        });

        return services;
    }
}
