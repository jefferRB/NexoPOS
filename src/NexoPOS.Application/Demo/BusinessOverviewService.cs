using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;
using NexoPOS.Domain.Activity;
using NexoPOS.Domain.Branches;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Arma las vistas de negocio a partir de los datos de demostración. Toda la
/// consolidación (totales, alertas, actividad) ocurre aquí para que los números
/// presentados sean coherentes con los datos individuales.
/// </summary>
public sealed class BusinessOverviewService(IDemoOperationsRepository repository, IReorderService reorderService) : IBusinessOverviewService
{
    public async Task<OverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default)
    {
        var branches = await repository.GetBranchesAsync(cancellationToken);
        var products = await repository.GetProductsAsync(cancellationToken);
        var stock = await repository.GetInventoryAsync(cancellationToken);
        var kits = await repository.GetMobileKitsAsync(cancellationToken);
        var activity = await repository.GetRecentActivityAsync(cancellationToken);
        var pendingTransfers = await repository.GetPendingTransferCountAsync(cancellationToken);
        var reorderCount = await reorderService.GetSuggestedCountAsync(ReorderBasis.Weekly, cancellationToken);

        var branchNames = branches.ToDictionary(b => b.Id, b => b.Name);
        var branchSummaries = branches
            .Select(b => BuildBranchSummary(b, products, stock, kits))
            .ToList();

        var indicators = new IndicatorsDto(
            SalesToday: branchSummaries.Sum(b => b.SalesToday),
            TicketsToday: branchSummaries.Sum(b => b.TicketsToday),
            LowStockProducts: branchSummaries.Sum(b => b.LowStockCount),
            ReorderSuggestedCount: reorderCount,
            ReceivablesTotal: branchSummaries.Sum(b => b.ReceivablesBalance),
            PayablesTotal: branchSummaries.Sum(b => b.PayablesBalance));

        var priorityAlerts = BuildPriorityAlerts(products, branches, stock, kits, pendingTransfers, reorderCount);

        var recentActivity = activity
            .OrderByDescending(e => e.Timestamp)
            .Select(e => MapActivity(e, branchNames, kits))
            .ToList();

        return new OverviewDto(indicators, branchSummaries, priorityAlerts, recentActivity);
    }

    public async Task<InventoryDto> GetInventoryAsync(CancellationToken cancellationToken = default)
    {
        var branches = await repository.GetBranchesAsync(cancellationToken);
        var products = await repository.GetProductsAsync(cancellationToken);
        var stock = await repository.GetInventoryAsync(cancellationToken);
        var kitStock = await repository.GetMobileKitStockAsync(cancellationToken);
        var suppliers = await repository.GetSuppliersAsync(cancellationToken);
        var supplierNames = suppliers.ToDictionary(s => s.Id, s => s.Name);

        var branchRefs = branches.Select(b => new BranchRefDto(b.Id, b.Name)).ToList();
        var items = products.Select(p => BuildInventoryItem(p, branches, stock, kitStock, supplierNames)).ToList();

        return new InventoryDto(branchRefs, items);
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(string productId, CancellationToken cancellationToken = default)
    {
        var products = await repository.GetProductsAsync(cancellationToken);
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product is null)
        {
            return null;
        }

        var branches = await repository.GetBranchesAsync(cancellationToken);
        var stock = await repository.GetInventoryAsync(cancellationToken);
        var kitStock = await repository.GetMobileKitStockAsync(cancellationToken);
        var suppliers = await repository.GetSuppliersAsync(cancellationToken);
        var supplierNames = suppliers.ToDictionary(s => s.Id, s => s.Name);

        var summary = BuildInventoryItem(product, branches, stock, kitStock, supplierNames);

        decimal? coverageDays = null;
        if (product.WeeklyAverageSales > 0)
        {
            var dailyRate = product.WeeklyAverageSales / 7m;
            coverageDays = decimal.Round(summary.Total / dailyRate, 1);
        }

        var reorderStatus = coverageDays is null ? "no-data" : coverageDays < 14m ? "needs-reorder" : "sufficient";

        return new ProductDetailDto(
            Summary: summary,
            PurchaseUnitLabel: product.Presentation?.PurchaseUnitLabel,
            BaseUnitsPerPurchaseUnit: product.Presentation?.BaseUnitsPerPurchaseUnit,
            WeeklyAverageSales: product.WeeklyAverageSales,
            MonthlyAverageSales: product.MonthlyAverageSales,
            CoverageDays: coverageDays,
            ReorderStatus: reorderStatus);
    }

    private static InventoryItemDto BuildInventoryItem(
        Product product,
        IReadOnlyList<Branch> branches,
        IReadOnlyList<InventoryStock> stock,
        IReadOnlyList<Domain.MobileKits.MobileKitStock> kitStock,
        IReadOnlyDictionary<string, string> supplierNames)
    {
        var stockByBranch = branches.ToDictionary(
            b => b.Id,
            b => stock.Where(s => s.ProductId == product.Id && s.BranchId == b.Id).Sum(s => s.Quantity));
        var mobileKitsStock = InventoryMath.MobileKitStock(product.Id, kitStock);
        var total = stockByBranch.Values.Sum() + mobileKitsStock;
        var status = StockStatusRules.Evaluate(total, product.MinimumStock);

        return new InventoryItemDto(
            ProductId: product.Id,
            Name: product.Name,
            InternalCode: product.Sku,
            ManufacturerBarcode: product.ManufacturerBarcode,
            Category: product.Category,
            Type: DemoPresentation.ProductTypeCode(product.Type),
            Unit: DemoPresentation.UnitCode(product.BaseUnit),
            IsFractionable: product.IsFractionable,
            SupplierId: product.SupplierId,
            SupplierName: supplierNames.GetValueOrDefault(product.SupplierId, product.SupplierId),
            StockByBranch: stockByBranch,
            MobileKitsStock: mobileKitsStock,
            Total: total,
            Minimum: product.MinimumStock,
            Status: DemoPresentation.StatusCode(status));
    }

    public async Task<BranchDetailDto?> GetBranchDetailAsync(string branchId, CancellationToken cancellationToken = default)
    {
        var branch = await repository.GetBranchAsync(branchId, cancellationToken);
        if (branch is null)
        {
            return null;
        }

        var products = await repository.GetProductsAsync(cancellationToken);
        var stock = await repository.GetInventoryAsync(cancellationToken);
        var activity = await repository.GetRecentActivityAsync(cancellationToken);
        var kits = await repository.GetMobileKitsAsync(cancellationToken);
        var weeklyPerformance = await repository.GetWeeklyPerformanceAsync(cancellationToken);
        var topProducts = await repository.GetTopProductsAsync(cancellationToken);

        var summary = BuildBranchSummary(branch, products, stock, kits);
        var productById = products.ToDictionary(p => p.Id);
        var branchNames = new Dictionary<string, string> { [branch.Id] = branch.Name };

        var inventory = stock
            .Where(s => s.BranchId == branchId && productById.ContainsKey(s.ProductId))
            .Select(s =>
            {
                var product = productById[s.ProductId];
                var status = StockStatusRules.Evaluate(s.Quantity, product.MinimumStock);
                return new BranchInventoryItemDto(
                    ProductId: product.Id,
                    Name: product.Name,
                    InternalCode: product.Sku,
                    Type: DemoPresentation.ProductTypeCode(product.Type),
                    Quantity: s.Quantity,
                    Unit: DemoPresentation.UnitCode(product.BaseUnit),
                    Minimum: product.MinimumStock,
                    Status: DemoPresentation.StatusCode(status));
            })
            .OrderBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var recentActivity = activity
            .Where(e => e.BranchId == branchId)
            .OrderByDescending(e => e.Timestamp)
            .Select(e => MapActivity(e, branchNames, kits))
            .ToList();

        var performance = weeklyPerformance
            .Where(p => p.BranchId == branchId)
            .OrderBy(p => p.Date)
            .Select(p => new DailyPerformanceDto(p.Date, p.Tickets, p.Sales))
            .ToList();

        var topProductDtos = topProducts
            .Where(t => t.BranchId == branchId && productById.ContainsKey(t.ProductId))
            .OrderByDescending(t => t.QuantitySold)
            .Select(t => new TopProductDto(
                productById[t.ProductId].Name,
                t.QuantitySold,
                DemoPresentation.UnitCode(productById[t.ProductId].BaseUnit)))
            .ToList();

        return new BranchDetailDto(summary, inventory, recentActivity, performance, topProductDtos);
    }

    private static BranchSummaryDto BuildBranchSummary(
        Branch branch,
        IReadOnlyList<Product> products,
        IReadOnlyList<InventoryStock> stock,
        IReadOnlyList<MobileKit> kits)
    {
        var productById = products.ToDictionary(p => p.Id);
        var branchStock = stock.Where(s => s.BranchId == branch.Id).ToList();

        var managedProductCount = branchStock.Count(s => s.Quantity > 0);
        var lowStockCount = branchStock.Count(s =>
            productById.TryGetValue(s.ProductId, out var product) &&
            StockStatusRules.IsAlert(StockStatusRules.Evaluate(s.Quantity, product.MinimumStock)));
        var mobileKitsCount = kits.Count(k => k.HomeBranchId == branch.Id);

        return new BranchSummaryDto(
            Id: branch.Id,
            Name: branch.Name,
            Location: branch.Location,
            Phone: branch.Phone,
            Schedule: branch.Schedule,
            IsOperational: branch.IsOperational,
            SalesToday: branch.SalesToday,
            TicketsToday: branch.TicketsToday,
            ManagedProductCount: managedProductCount,
            LowStockCount: lowStockCount,
            ActiveCollaborators: branch.ActiveCollaborators,
            MobileKitsCount: mobileKitsCount,
            ReceivablesBalance: branch.ReceivablesBalance,
            PayablesBalance: branch.PayablesBalance);
    }

    /// <summary>
    /// Arma un puñado de alertas destacadas y heterogéneas para el panel
    /// principal: existencias, botiquines, transferencias y reposición. Se
    /// eligen de forma determinística (primer producto/sucursal en orden de
    /// catálogo) para no depender de identificadores concretos.
    /// </summary>
    private static IReadOnlyList<PriorityAlertDto> BuildPriorityAlerts(
        IReadOnlyList<Product> products,
        IReadOnlyList<Branch> branches,
        IReadOnlyList<InventoryStock> stock,
        IReadOnlyList<MobileKit> kits,
        int pendingTransfers,
        int reorderCount)
    {
        var alerts = new List<PriorityAlertDto>();
        var branchNames = branches.ToDictionary(b => b.Id, b => b.Name);

        var medicationLow = FindFirstStockAlert(products, ProductType.Medication, StockStatus.Low, branches, stock);
        if (medicationLow is not null)
        {
            alerts.Add(ToStockAlert("alert-med-low", medicationLow.Value, branchNames, "medium", "próximo al mínimo"));
        }

        var medicationOut = FindFirstStockAlert(products, ProductType.Medication, StockStatus.OutOfStock, branches, stock);
        if (medicationOut is not null)
        {
            alerts.Add(ToStockAlert("alert-med-out", medicationOut.Value, branchNames, "high", "agotado"));
        }

        var foodAlert = FindFirstStockAlert(products, ProductType.Food, null, branches, stock);
        if (foodAlert is not null)
        {
            var (product, _, status) = foodAlert.Value;
            alerts.Add(ToStockAlert(
                "alert-food",
                foodAlert.Value,
                branchNames,
                status == StockStatus.OutOfStock ? "high" : "medium",
                status == StockStatus.OutOfStock ? "agotado" : "próximo al mínimo"));
        }

        var kitNeedingReview = kits.FirstOrDefault(k => k.Status == MobileKitStatus.NeedsReview);
        if (kitNeedingReview is not null)
        {
            alerts.Add(new PriorityAlertDto(
                Id: "alert-kit",
                Category: "mobile-kit",
                Title: "Botiquín pendiente de conciliación",
                Description: $"{kitNeedingReview.Name} ({kitNeedingReview.AssignedTo}) requiere revisión.",
                Severity: "medium",
                BranchId: kitNeedingReview.HomeBranchId,
                BranchName: branchNames.GetValueOrDefault(kitNeedingReview.HomeBranchId)));
        }

        if (pendingTransfers > 0)
        {
            alerts.Add(new PriorityAlertDto(
                Id: "alert-transfer",
                Category: "transfer",
                Title: "Transferencia pendiente",
                Description: $"{pendingTransfers} transferencia(s) entre sucursales están en tránsito.",
                Severity: "low",
                BranchId: null,
                BranchName: null));
        }

        if (reorderCount > 0)
        {
            alerts.Add(new PriorityAlertDto(
                Id: "alert-reorder",
                Category: "reorder",
                Title: "Producto con reposición sugerida",
                Description: $"{reorderCount} producto(s) requieren reposición esta semana.",
                Severity: "low",
                BranchId: null,
                BranchName: null));
        }

        return alerts;
    }

    private static (Product Product, string BranchId, StockStatus Status)? FindFirstStockAlert(
        IReadOnlyList<Product> products,
        ProductType type,
        StockStatus? specificStatus,
        IReadOnlyList<Branch> branches,
        IReadOnlyList<InventoryStock> stock)
    {
        foreach (var product in products.Where(p => p.Type == type))
        {
            foreach (var branch in branches)
            {
                var quantity = stock
                    .Where(s => s.ProductId == product.Id && s.BranchId == branch.Id)
                    .Select(s => s.Quantity)
                    .FirstOrDefault();
                var status = StockStatusRules.Evaluate(quantity, product.MinimumStock);

                if (status == StockStatus.Available)
                {
                    continue;
                }
                if (specificStatus.HasValue && status != specificStatus.Value)
                {
                    continue;
                }

                return (product, branch.Id, status);
            }
        }

        return null;
    }

    private static PriorityAlertDto ToStockAlert(
        string id,
        (Product Product, string BranchId, StockStatus Status) alert,
        IReadOnlyDictionary<string, string> branchNames,
        string severity,
        string statusLabel)
    {
        var branchName = branchNames.GetValueOrDefault(alert.BranchId, alert.BranchId);
        var category = alert.Status == StockStatus.OutOfStock ? "stock-out" : "stock-low";
        return new PriorityAlertDto(
            Id: id,
            Category: category,
            Title: $"{alert.Product.Name} {statusLabel}",
            Description: $"{alert.Product.Name} está {statusLabel} en {branchName}.",
            Severity: severity,
            BranchId: alert.BranchId,
            BranchName: branchName);
    }

    private static ActivityDto MapActivity(
        ActivityEvent e,
        IReadOnlyDictionary<string, string> branchNames,
        IReadOnlyList<MobileKit> kits)
    {
        var locationName = e.MobileKitId is not null
            ? kits.FirstOrDefault(k => k.Id == e.MobileKitId)?.Name ?? e.MobileKitId
            : branchNames.GetValueOrDefault(e.BranchId, e.BranchId);

        return new ActivityDto(
            Id: e.Id,
            Type: DemoPresentation.ActivityCode(e.Type),
            Action: DemoPresentation.ActivityLabel(e.Type),
            UserName: e.UserName,
            UserRole: e.UserRole,
            LocationName: locationName,
            BranchId: e.BranchId,
            MobileKitId: e.MobileKitId,
            Timestamp: e.Timestamp,
            Reference: e.Reference,
            Reason: e.Reason,
            Amount: e.Amount);
    }
}
