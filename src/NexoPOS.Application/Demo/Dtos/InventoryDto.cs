namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Inventario consolidado: sucursales disponibles y renglones de producto.</summary>
public sealed record InventoryDto(
    IReadOnlyList<BranchRefDto> Branches,
    IReadOnlyList<InventoryItemDto> Items);

/// <summary>Referencia mínima a una sucursal (para encabezados de columnas).</summary>
public sealed record BranchRefDto(string Id, string Name);

/// <summary>
/// Renglón de inventario consolidado. <see cref="StockByBranch"/> está indexado
/// por el identificador de sucursal. Las cantidades están en <see cref="Unit"/>;
/// nunca se suman unidades incompatibles.
/// </summary>
public sealed record InventoryItemDto(
    string ProductId,
    string Name,
    string InternalCode,
    string ManufacturerBarcode,
    string Category,
    string Type,
    string Unit,
    bool IsFractionable,
    string SupplierId,
    string SupplierName,
    IReadOnlyDictionary<string, decimal> StockByBranch,
    decimal MobileKitsStock,
    decimal Total,
    decimal Minimum,
    string Status);

/// <summary>Detalle ampliado de un producto para el modal de inventario.</summary>
public sealed record ProductDetailDto(
    InventoryItemDto Summary,
    string? PurchaseUnitLabel,
    decimal? BaseUnitsPerPurchaseUnit,
    decimal WeeklyAverageSales,
    decimal MonthlyAverageSales,
    decimal? CoverageDays,
    string ReorderStatus);
