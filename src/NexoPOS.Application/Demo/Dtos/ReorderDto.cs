namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Sugerencia de reposición de un producto, calculada a partir del consumo promedio.</summary>
public sealed record ReorderSuggestionDto(
    string ProductId,
    string ProductName,
    string ProductCode,
    string SupplierId,
    string SupplierName,
    decimal CurrentStock,
    string Unit,
    decimal WeeklyAverageSales,
    decimal MonthlyAverageSales,
    decimal CoverageDays,
    decimal SuggestedQuantity,
    string Priority);

/// <summary>Propuesta de orden de compra agrupada por proveedor.</summary>
public sealed record SupplierOrderDto(
    string SupplierId,
    string SupplierName,
    decimal EstimatedValue,
    IReadOnlyList<ReorderSuggestionDto> Items);

/// <summary>Indicadores del reporte de reposición.</summary>
public sealed record ReorderIndicatorsDto(
    int ProductsToReorder,
    int SuppliersInvolved,
    decimal EstimatedValue,
    decimal AverageCoverageDays);

/// <summary>Respuesta del reporte de reposición para una base de cálculo (semanal o mensual).</summary>
public sealed record ReorderResponseDto(
    string Basis,
    ReorderIndicatorsDto Indicators,
    IReadOnlyList<SupplierOrderDto> SupplierOrders);
