using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Infrastructure.Demo;

namespace NexoPOS.Infrastructure;

/// <summary>Registro de los servicios de la capa de Infraestructura.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Datos de demostración en memoria. Singleton: son estáticos y de solo lectura.
        // Cuando exista una base de datos real, aquí se registrará el DbContext usando
        // la cadena de conexión de 'configuration'.
        _ = configuration;
        services.AddSingleton<IDemoOperationsRepository, DemoOperationsRepository>();
        return services;
    }
}
