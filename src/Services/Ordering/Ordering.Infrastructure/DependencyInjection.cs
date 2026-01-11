using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services
        , IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        // In order to make the .net framework DI tool to resolve the dependencies of
        // the interceptors, we are adding the interceptors as scoped services
        /*
         The commented code below won't work because the interceptors won't have their dependencies injected
         services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.AddInterceptors(new AuditableEntityInterceptor()\
                                , new DispatchDomainEventsInterceptor(<somehow_Imediator>));
                options.UseSqlServer(connectionString);
            });
         */
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // this way the interceptors will have their dependencies injected by .net framework DI tool
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        return services;
    }
}
