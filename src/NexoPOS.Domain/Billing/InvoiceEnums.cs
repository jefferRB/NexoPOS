namespace NexoPOS.Domain.Billing;

/// <summary>Tipo de comprobante electrónico emitido (modelo de demostración).</summary>
public enum InvoiceDocumentType
{
    ElectronicInvoice,
    ElectronicTicket
}

public enum InvoicePaymentMethod
{
    Cash,
    Card,
    SinpeMovil,
    BankTransfer,
    Credit,
    Mixed
}

/// <summary>Estado fiscal de demostración del comprobante (no representa Hacienda real).</summary>
public enum InvoiceStatus
{
    Accepted,
    Pending,
    Voided
}
