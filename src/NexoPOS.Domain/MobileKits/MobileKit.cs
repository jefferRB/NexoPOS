namespace NexoPOS.Domain.MobileKits;

/// <summary>
/// Un botiquín móvil asignado a un veterinario para visitas a domicilio. El
/// inventario que transporta se audita por separado (<see cref="MobileKitStock"/>).
/// </summary>
public sealed class MobileKit
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string AssignedTo { get; init; }
    public required string HomeBranchId { get; init; }
    public MobileKitStatus Status { get; init; }
    public DateTimeOffset? LastTransferAt { get; init; }
    public DateTimeOffset? LastConsumptionAt { get; init; }

    /// <summary>Alertas puntuales del botiquín (por ejemplo, diferencias por conciliar).</summary>
    public IReadOnlyList<string> Alerts { get; init; } = [];
}
