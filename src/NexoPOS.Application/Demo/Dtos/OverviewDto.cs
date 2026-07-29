namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Respuesta del resumen general de la empresa.</summary>
public sealed record OverviewDto(
    IndicatorsDto Indicators,
    IReadOnlyList<BranchSummaryDto> Branches,
    IReadOnlyList<PriorityAlertDto> PriorityAlerts,
    IReadOnlyList<ActivityDto> RecentActivity);

/// <summary>Indicadores generales consolidados.</summary>
public sealed record IndicatorsDto(
    decimal SalesToday,
    int TicketsToday,
    int LowStockProducts,
    int ReorderSuggestedCount,
    decimal ReceivablesTotal,
    decimal PayablesTotal);

/// <summary>Resumen de una sucursal para tarjetas y encabezados.</summary>
public sealed record BranchSummaryDto(
    string Id,
    string Name,
    string Location,
    string Phone,
    string Schedule,
    bool IsOperational,
    decimal SalesToday,
    int TicketsToday,
    int ManagedProductCount,
    int LowStockCount,
    int ActiveCollaborators,
    int MobileKitsCount,
    decimal ReceivablesBalance,
    decimal PayablesBalance);

/// <summary>
/// Alerta destacada del panel principal. A diferencia de una alerta de stock
/// simple, agrupa distintos tipos de aviso (existencias, botiquines,
/// transferencias, reposición) bajo una misma forma.
/// </summary>
public sealed record PriorityAlertDto(
    string Id,
    string Category,
    string Title,
    string Description,
    string Severity,
    string? BranchId,
    string? BranchName);

/// <summary>Evento de la actividad reciente, con trazabilidad completa.</summary>
public sealed record ActivityDto(
    string Id,
    string Type,
    string Action,
    string UserName,
    string UserRole,
    string LocationName,
    string? BranchId,
    string? MobileKitId,
    DateTimeOffset Timestamp,
    string Reference,
    string Reason,
    decimal? Amount);
