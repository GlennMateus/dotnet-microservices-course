using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services
        , IConfiguration configuration
        , Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            // Set case formatter to kebab-case for endpoint names
            // https://developer.mozilla.org/en-US/docs/Glossary/Kebab_case
            // https://www.tuple.nl/en/knowledge-base/kebab-case
            config.SetKebabCaseEndpointNameFormatter();

            // When null -> It means that it is a publisher
            // When not null -> It means that it is a consumer
            if (assembly != null)
                config.AddConsumers(assembly);
            
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri($"{configuration["MessageBroker:Host"]!}:{configuration["MessageBroker:Port"]!}")
                    , host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
