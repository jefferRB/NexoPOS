namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Detalle de una sucursal: resumen, inventario propio, movimientos y desempeño.</summary>
public sealed record BranchDetailDto(
    BranchSummaryDto Branch,
    IReadOnlyList<BranchInventoryItemDto> Inventory,
    IReadOnlyList<ActivityDto> RecentActivity,
    IReadOnlyList<DailyPerformanceDto> WeeklyPerformance,
    IReadOnlyList<TopProductDto> TopProducts);

/// <summary>Renglón de inventario de una sucursal concreta.</summary>
public sealed record BranchInventoryItemDto(
    string ProductId,
    string Name,
    string InternalCode,
    string Type,
    decimal Quantity,
    string Unit,
    decimal Minimum,
    string Status);

/// <summary>Un día del desempeño de los últimos 7 días de la sucursal.</summary>
public sealed record DailyPerformanceDto(DateOnly Date, int Tickets, decimal Sales);

/// <summary>Producto más vendido de la sucursal durante la semana (dato ilustrativo).</summary>
public sealed record TopProductDto(string ProductName, decimal QuantitySold, string Unit);
