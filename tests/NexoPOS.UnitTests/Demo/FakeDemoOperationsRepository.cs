using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Domain.Activity;
using NexoPOS.Domain.Billing;
using NexoPOS.Domain.Branches;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;
using NexoPOS.Domain.Services;

namespace NexoPOS.UnitTests.Demo;

/// <summary>
/// Repositorio falso con datos pequeños y conocidos para probar la lógica de
/// consolidación de los servicios de aplicación de forma aislada (sin la
/// Infraestructura veterinaria real).
///
/// Totales esperados:
///   Tickets -> b1=10, b2=20, b3=15 (total 45)
///   Ventas -> 100 + 200 + 300 = 600
///   Existencias de pA (Medicamento, min 10 ml): b1=50, b2=8 (bajo), b3=0 (agotado), botiquín kitZ=5 -> total 63
///   Existencias de pB (Insumo, min 5 uds): b1=3 (bajo), b2=20, b3=15 -> total 38
///   Alertas por sucursal -> b1: pB bajo (1); b2: pA bajo (1); b3: pA agotado (1) -> total 3
/// </summary>
internal sealed class FakeDemoOperationsRepository : IDemoOperationsRepository
{
    public const string SupplierId = "sX";
    public const string ServiceId = "svcT";
    public const string MobileKitId = "kitZ";

    private static readonly IReadOnlyList<Branch> Branches =
    [
        new Branch { Id = "b1", Name = "Sucursal Uno", Location = "Ubicación 1", Phone = "0000-0001", Schedule = "8:00 a. m. - 5:00 p. m.", IsOperational = true, ActiveCollaborators = 2, SalesToday = 100m, TicketsToday = 10, ReceivablesBalance = 50m, PayablesBalance = 30m },
        new Branch { Id = "b2", Name = "Sucursal Dos", Location = "Ubicación 2", Phone = "0000-0002", Schedule = "8:00 a. m. - 5:00 p. m.", IsOperational = true, ActiveCollaborators = 3, SalesToday = 200m, TicketsToday = 20, ReceivablesBalance = 80m, PayablesBalance = 60m },
        new Branch { Id = "b3", Name = "Sucursal Tres", Location = "Ubicación 3", Phone = "0000-0003", Schedule = "8:00 a. m. - 5:00 p. m.", IsOperational = false, ActiveCollaborators = 1, SalesToday = 300m, TicketsToday = 15, ReceivablesBalance = 40m, PayablesBalance = 20m }
    ];

    private static readonly IReadOnlyList<Supplier> Suppliers =
    [
        new Supplier { Id = SupplierId, Name = "Proveedor X", ContactPhone = "0000-9999" }
    ];

    private static readonly IReadOnlyList<Product> Products =
    [
        new Product
        {
            Id = "pA", Sku = "SKU-A", ManufacturerBarcode = "0001", Name = "Producto A", Category = "Cat",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Milliliter, MinimumStock = 10m,
            UnitPrice = 100m, SupplierId = SupplierId,
            Presentation = new ProductPresentation { PurchaseUnitLabel = "Frasco 50 ml", BaseUnitsPerPurchaseUnit = 50m },
            WeeklyAverageSales = 70m, MonthlyAverageSales = 300m
        },
        new Product
        {
            Id = "pB", Sku = "SKU-B", ManufacturerBarcode = "0002", Name = "Producto B", Category = "Cat",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit, MinimumStock = 5m,
            UnitPrice = 20m, SupplierId = SupplierId,
            Presentation = null, WeeklyAverageSales = 0m, MonthlyAverageSales = 0m
        }
    ];

    private static readonly IReadOnlyList<InventoryStock> Inventory =
    [
        new InventoryStock { ProductId = "pA", BranchId = "b1", Quantity = 50m },
        new InventoryStock { ProductId = "pA", BranchId = "b2", Quantity = 8m },
        new InventoryStock { ProductId = "pA", BranchId = "b3", Quantity = 0m },
        new InventoryStock { ProductId = "pB", BranchId = "b1", Quantity = 3m },
        new InventoryStock { ProductId = "pB", BranchId = "b2", Quantity = 20m },
        new InventoryStock { ProductId = "pB", BranchId = "b3", Quantity = 15m }
    ];

    private static readonly IReadOnlyList<MobileKit> MobileKits =
    [
        new MobileKit { Id = MobileKitId, Name = "Botiquín Z", AssignedTo = "Dra. Prueba", HomeBranchId = "b2", Status = MobileKitStatus.Available, Alerts = [] }
    ];

    private static readonly IReadOnlyList<MobileKitStock> MobileKitStockList =
    [
        new MobileKitStock { MobileKitId = MobileKitId, ProductId = "pA", Quantity = 5m }
    ];

    private static readonly IReadOnlyList<ServiceDefinition> Services =
    [
        new ServiceDefinition
        {
            Id = ServiceId, Name = "Servicio de prueba", Description = "Servicio para pruebas.",
            DurationMinutes = 20, Price = 500m,
            Components =
            [
                new ServiceComponent { ProductId = "pA", Label = "Producto A", Quantity = 2m, Unit = MeasurementUnit.Milliliter },
                new ServiceComponent { Label = "Insumo genérico" },
                new ServiceComponent { Label = "Tiempo de veterinario", DurationMinutes = 20 }
            ]
        }
    ];

    private static readonly IReadOnlyList<DemoInvoice> Invoices =
    [
        new DemoInvoice
        {
            Id = "inv-1", Number = "TE-B1-0001", IssuedAt = DateTimeOffset.UtcNow.AddMinutes(-10),
            BranchId = "b1", CustomerName = "Cliente de prueba", IssuedBy = "Usuario 1",
            Type = InvoiceDocumentType.ElectronicTicket, PaymentMethod = InvoicePaymentMethod.Cash,
            Status = InvoiceStatus.Accepted, Total = 200m,
            Lines = [new DemoInvoiceLine { Description = "Producto A", Quantity = 2m, Unit = MeasurementUnit.Milliliter, UnitPrice = 100m, LineTotal = 200m }]
        },
        new DemoInvoice
        {
            Id = "inv-2", Number = "FE-B2-0001", IssuedAt = DateTimeOffset.UtcNow.AddMinutes(-30),
            BranchId = "b2", CustomerName = "Cliente de mostrador", IssuedBy = "Usuario 2",
            Type = InvoiceDocumentType.ElectronicInvoice, PaymentMethod = InvoicePaymentMethod.Card,
            Status = InvoiceStatus.Pending, Total = 500m,
            Lines = [new DemoInvoiceLine { Description = "Servicio de prueba", Quantity = 1m, Unit = MeasurementUnit.Unit, UnitPrice = 500m, LineTotal = 500m }]
        }
    ];

    private static readonly IReadOnlyList<ActivityEvent> Activity =
    [
        new ActivityEvent { Id = "e1", Type = ActivityType.InvoiceIssued, UserName = "Usuario 1", UserRole = "Cajera", BranchId = "b1", Timestamp = DateTimeOffset.UtcNow.AddMinutes(-5), Reference = "TE-B1-0001", Reason = "Venta de mostrador", Amount = 200m },
        new ActivityEvent { Id = "e2", Type = ActivityType.TransferToMobileKit, UserName = "Usuario 2", UserRole = "Veterinaria", BranchId = "b2", MobileKitId = MobileKitId, Timestamp = DateTimeOffset.UtcNow.AddMinutes(-15), Reference = "TRK-0001", Reason = "Reposición de ruta", Amount = null }
    ];

    private static readonly IReadOnlyList<BranchDailyPerformance> Performance =
    [
        new BranchDailyPerformance { BranchId = "b1", Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), Tickets = 8, Sales = 90m },
        new BranchDailyPerformance { BranchId = "b1", Date = DateOnly.FromDateTime(DateTime.UtcNow), Tickets = 10, Sales = 100m }
    ];

    private static readonly IReadOnlyList<BranchTopProduct> TopProducts =
    [
        new BranchTopProduct { BranchId = "b1", ProductId = "pA", QuantitySold = 12m }
    ];

    private const int PendingTransfers = 1;

    public Task<IReadOnlyList<Branch>> GetBranchesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Branches);

    public Task<Branch?> GetBranchAsync(string branchId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Branches.FirstOrDefault(b => b.Id == branchId));

    public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Products);

    public Task<IReadOnlyList<Supplier>> GetSuppliersAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Suppliers);

    public Task<IReadOnlyList<InventoryStock>> GetInventoryAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Inventory);

    public Task<IReadOnlyList<ActivityEvent>> GetRecentActivityAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Activity);

    public Task<int> GetPendingTransferCountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(PendingTransfers);

    public Task<IReadOnlyList<ServiceDefinition>> GetServicesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Services);

    public Task<IReadOnlyList<MobileKit>> GetMobileKitsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(MobileKits);

    public Task<IReadOnlyList<MobileKitStock>> GetMobileKitStockAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(MobileKitStockList);

    public Task<IReadOnlyList<DemoInvoice>> GetInvoicesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Invoices);

    public Task<DemoInvoice?> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default) =>
        Task.FromResult(Invoices.FirstOrDefault(i => i.Id == invoiceId));

    public Task<IReadOnlyList<BranchDailyPerformance>> GetWeeklyPerformanceAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(Performance);

    public Task<IReadOnlyList<BranchTopProduct>> GetTopProductsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(TopProducts);
}
