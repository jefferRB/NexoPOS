namespace NexoPOS.Domain.Activity;

/// <summary>Tipo de movimiento registrado en la operación.</summary>
public enum ActivityType
{
    InvoiceIssued,
    FractionalSale,
    ServiceBilled,
    InventoryAdjustment,
    PurchaseReception,
    TransferSent,
    TransferReceived,
    TransferToMobileKit,
    ReceivablePayment,
    CashClosing
}

/// <summary>
/// Un evento de la actividad reciente. Para trazabilidad se registra siempre el
/// usuario, su rol, la ubicación (sucursal o botiquín móvil) y el motivo.
/// </summary>
public sealed class ActivityEvent
{
    public required string Id { get; init; }
    public ActivityType Type { get; init; }
    public required string UserName { get; init; }
    public required string UserRole { get; init; }
    public required string BranchId { get; init; }

    /// <summary>Presente cuando el movimiento ocurrió en un botiquín móvil.</summary>
    public string? MobileKitId { get; init; }

    public DateTimeOffset Timestamp { get; init; }
    public required string Reference { get; init; }
    public required string Reason { get; init; }

    /// <summary>Monto en colones (CRC) cuando el evento lo tiene.</summary>
    public decimal? Amount { get; init; }
}
