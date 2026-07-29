using Microsoft.Extensions.DependencyInjection;
using NexoPOS.Application.Demo;
using NexoPOS.Application.Demo.Abstractions;

namespace NexoPOS.Application;

/// <summary>Registro de los servicios de la capa de Aplicación.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IReorderService, ReorderService>();
        services.AddScoped<IBusinessOverviewService, BusinessOverviewService>();
        services.AddScoped<IServicesCatalogService, ServicesCatalogService>();
        services.AddScoped<IMobileKitsService, MobileKitsService>();
        services.AddScoped<IInvoicingService, InvoicingService>();
        return services;
    }
}
