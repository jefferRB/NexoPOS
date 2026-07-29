using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Domain.Activity;
using NexoPOS.Domain.Billing;
using NexoPOS.Domain.Branches;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;
using NexoPOS.Domain.Services;
using NexoPOS.Infrastructure.Demo.Seed;

namespace NexoPOS.Infrastructure.Demo;

/// <summary>
/// Repositorio de demostración con datos en memoria (sin base de datos), para
/// Grupo Veterinario Demo. Los datos son coherentes: los totales por sucursal y
/// consolidados se derivan de las existencias individuales definidas en
/// <c>Infrastructure/Demo/Seed</c>. Importes en colones (CRC).
/// </summary>
public sealed class DemoOperationsRepository : IDemoOperationsRepository
{
    private static readonly IReadOnlyList<MobileKit> MobileKits = MobileKitSeed.Build(DateTimeOffset.UtcNow);
    private static readonly IReadOnlyList<MobileKitStock> MobileKitStockList = MobileKitSeed.BuildStock();
    private static readonly IReadOnlyList<ActivityEvent> Activity = ActivitySeed.Build(DateTimeOffset.UtcNow);

    private static readonly IReadOnlyList<DemoInvoice> Invoices = InvoiceSeed.Build(
        ProductSeed.Products.ToDictionary(p => p.Id),
        ServiceSeed.Services.ToDictionary(s => s.Id),
        DateTimeOffset.UtcNow);

    private const int PendingTransfers = 2;

    public Task<IReadOnlyList<Branch>> GetBranchesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(BranchSeed.Branches);

    public Task<Branch?> GetBranchAsync(string branchId, CancellationToken cancellationToken = default) =>
        Task.FromResult(BranchSeed.Branches.FirstOrDefault(b =>
            string.Equals(b.Id, branchId, StringComparison.OrdinalIgnoreCase)));

    public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(ProductSeed.Products);

    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(SupplierSeed.Suppliers);

    public Task<IReadOnlyList<InventoryStock>> GetInventoryAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(InventorySeed.Stock);

    public Task<IReadOnlyList<ActivityEvent>> GetRecentActivityAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Activity);

    public Task<int> GetPendingTransferCountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(PendingTransfers);

    public Task<IReadOnlyList<ServiceDefinition>> GetServicesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(ServiceSeed.Services);

    public Task<IReadOnlyList<MobileKit>> GetMobileKitsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(MobileKits);

    public Task<IReadOnlyList<MobileKitStock>> GetMobileKitStockAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(MobileKitStockList);

    public Task<IReadOnlyList<DemoInvoice>> GetInvoicesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Invoices);

    public Task<DemoInvoice?> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Invoices.FirstOrDefault(i =>
            string.Equals(i.Id, invoiceId, StringComparison.OrdinalIgnoreCase)));

    public Task<IReadOnlyList<Domain.Branches.BranchDailyPerformance>> GetWeeklyPerformanceAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(PerformanceSeed.Build(DateOnly.FromDateTime(DateTime.UtcNow)));

    public Task<IReadOnlyList<Domain.Branches.BranchTopProduct>> GetTopProductsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(PerformanceSeed.TopProducts);
}
