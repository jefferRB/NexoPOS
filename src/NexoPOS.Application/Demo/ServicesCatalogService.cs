using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Expone el catálogo de servicios compuestos. Un componente "enlaza con
/// inventario" cuando tiene un producto real asociado: al facturar el servicio,
/// ese componente descontaría existencias automáticamente.
/// </summary>
public sealed class ServicesCatalogService(IDemoOperationsRepository repository) : IServicesCatalogService
{
    public async Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        var services = await repository.GetServicesAsync(cancellationToken);

        return services
            .Select(service => new ServiceDto(
                Id: service.Id,
                Name: service.Name,
                Description: service.Description,
                DurationMinutes: service.DurationMinutes,
                Price: service.Price,
                Components: service.Components
                    .Select(c => new ServiceComponentDto(
                        ProductId: c.ProductId,
                        Label: c.Label,
                        Quantity: c.Quantity,
                        Unit: c.Unit is null ? null : DemoPresentation.UnitCode(c.Unit.Value),
                        DurationMinutes: c.DurationMinutes,
                        LinksToInventory: c.ProductId is not null))
                    .ToList()))
            .ToList();
    }
}
