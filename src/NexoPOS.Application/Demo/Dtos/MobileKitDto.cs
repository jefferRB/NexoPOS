namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Renglón de existencias dentro de un botiquín móvil.</summary>
public sealed record MobileKitStockLineDto(
    string ProductId,
    string ProductName,
    decimal Quantity,
    string Unit,
    decimal EstimatedValue);

/// <summary>Un botiquín móvil asignado a un veterinario de visita a domicilio.</summary>
public sealed record MobileKitDto(
    string Id,
    string Name,
    string AssignedTo,
    string HomeBranchId,
    string HomeBranchName,
    string Status,
    DateTimeOffset? LastTransferAt,
    DateTimeOffset? LastConsumptionAt,
    decimal EstimatedValue,
    IReadOnlyList<string> Alerts,
    IReadOnlyList<MobileKitStockLineDto> Stock,
    IReadOnlyList<ActivityDto> RecentActivity);
