using NexoPOS.Domain.Activity;
using NexoPOS.Domain.Billing;
using NexoPOS.Domain.Branches;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;
using NexoPOS.Domain.Services;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>
/// Fuente de datos de demostración. La implementación concreta vive en la capa
/// de Infraestructura y mantiene los datos en memoria (sin base de datos).
/// </summary>
public interface IDemoOperationsRepository
{
    Task<IReadOnlyList<Branch>> GetBranchesAsync(CancellationToken cancellationToken = default);

    Task<Branch?> GetBranchAsync(string branchId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryStock>> GetInventoryAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ActivityEvent>> GetRecentActivityAsync(CancellationToken cancellationToken = default);

    Task<int> GetPendingTransferCountAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ServiceDefinition>> GetServicesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MobileKit>> GetMobileKitsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MobileKitStock>> GetMobileKitStockAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DemoInvoice>> GetInvoicesAsync(CancellationToken cancellationToken = default);

    Task<DemoInvoice?> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BranchDailyPerformance>> GetWeeklyPerformanceAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BranchTopProduct>> GetTopProductsAsync(CancellationToken cancellationToken = default);
}
