using NexoPOS.Domain.Catalog;

namespace NexoPOS.Domain.Billing;

/// <summary>Línea de un comprobante: un producto o un servicio facturado.</summary>
public sealed class DemoInvoiceLine
{
    public required string Description { get; init; }
    public decimal Quantity { get; init; }
    public MeasurementUnit Unit { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }
}
