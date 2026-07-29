using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>
/// Servicio de aplicación que arma las vistas de negocio a partir de los datos
/// de demostración: resumen general, inventario consolidado y detalle de sucursal.
/// </summary>
public interface IBusinessOverviewService
{
    Task<OverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default);

    Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken = default);

    /// <summary>Devuelve el detalle de la sucursal, o <c>null</c> si no existe.</summary>
    Task<BranchDetailDto?> GetBranchDetailAsync(string branchId, CancellationToken cancellationToken = default);

    /// <summary>Devuelve el detalle ampliado de un producto, o <c>null</c> si no existe.</summary>
    Task<ProductDetailDto?> GetProductDetailAsync(string productId, CancellationToken cancellationToken = default);
}
