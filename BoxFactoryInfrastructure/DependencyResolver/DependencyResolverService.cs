using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BoxFactoryInfrastructure.DependencyResolver;

public class DependencyResolverService
{
    public static void RegisterInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IBoxRepository, BoxRepository>();
    }
} 