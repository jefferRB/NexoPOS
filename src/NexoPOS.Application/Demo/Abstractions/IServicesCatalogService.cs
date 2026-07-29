using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>Catálogo de servicios y paquetes compuestos ofrecidos por la veterinaria.</summary>
public interface IServicesCatalogService
{
    Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken cancellationToken = default);
}
