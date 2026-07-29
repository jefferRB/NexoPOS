namespace NexoPOS.Domain.Billing;

/// <summary>
/// Comprobante de demostración. No representa una factura electrónica real: no
/// tiene clave, XML ni consecutivo oficial de Hacienda.
/// </summary>
public sealed class DemoInvoice
{
    public required string Id { get; init; }
    public required string Number { get; init; }
    public DateTimeOffset IssuedAt { get; init; }
    public required string BranchId { get; init; }
    public required string CustomerName { get; init; }
    public required string IssuedBy { get; init; }
    public InvoiceDocumentType Type { get; init; }
    public InvoicePaymentMethod PaymentMethod { get; init; }
    public InvoiceStatus Status { get; init; }
    public decimal Total { get; init; }
    public required IReadOnlyList<DemoInvoiceLine> Lines { get; init; }
}
