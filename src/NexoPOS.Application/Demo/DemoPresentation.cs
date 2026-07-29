using NexoPOS.Domain.Activity;
using NexoPOS.Domain.Billing;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Traduce los enums del dominio a las cadenas estables que consume el frontend.
/// Se mantienen aparte para no ensuciar los servicios con detalles de presentación.
/// </summary>
internal static class DemoPresentation
{
    public static string StatusCode(StockStatus status) => status switch
    {
        StockStatus.Available => "available",
        StockStatus.Low => "low",
        StockStatus.OutOfStock => "out",
        _ => "available"
    };

    public static string UnitCode(MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Unit => "unit",
        MeasurementUnit.Milliliter => "ml",
        MeasurementUnit.Kilogram => "kg",
        _ => "unit"
    };

    public static string ProductTypeCode(ProductType type) => type switch
    {
        ProductType.Standard => "standard",
        ProductType.Medication => "medication",
        ProductType.Food => "food",
        ProductType.ClinicalSupply => "clinical-supply",
        _ => "standard"
    };

    public static string ActivityCode(ActivityType type) => type switch
    {
        ActivityType.InvoiceIssued => "invoice-issued",
        ActivityType.FractionalSale => "fractional-sale",
        ActivityType.ServiceBilled => "service-billed",
        ActivityType.InventoryAdjustment => "adjustment",
        ActivityType.PurchaseReception => "purchase",
        ActivityType.TransferSent => "transfer-out",
        ActivityType.TransferReceived => "transfer-in",
        ActivityType.TransferToMobileKit => "transfer-kit",
        ActivityType.ReceivablePayment => "receivable-payment",
        ActivityType.CashClosing => "cash-closing",
        _ => "activity"
    };

    public static string ActivityLabel(ActivityType type) => type switch
    {
        ActivityType.InvoiceIssued => "Factura electrónica emitida",
        ActivityType.FractionalSale => "Venta por fracción",
        ActivityType.ServiceBilled => "Servicio facturado",
        ActivityType.InventoryAdjustment => "Ajuste autorizado",
        ActivityType.PurchaseReception => "Recepción de compra",
        ActivityType.TransferSent => "Transferencia enviada",
        ActivityType.TransferReceived => "Transferencia recibida",
        ActivityType.TransferToMobileKit => "Transferencia hacia botiquín",
        ActivityType.ReceivablePayment => "Abono de cuenta por cobrar",
        ActivityType.CashClosing => "Cierre de caja",
        _ => "Movimiento"
    };

    public static string MobileKitStatusCode(MobileKitStatus status) => status switch
    {
        MobileKitStatus.OnRoute => "on-route",
        MobileKitStatus.Available => "available",
        MobileKitStatus.NeedsReview => "needs-review",
        _ => "available"
    };

    public static string InvoiceTypeCode(InvoiceDocumentType type) => type switch
    {
        InvoiceDocumentType.ElectronicInvoice => "electronic-invoice",
        InvoiceDocumentType.ElectronicTicket => "electronic-ticket",
        _ => "electronic-ticket"
    };

    public static string PaymentMethodCode(InvoicePaymentMethod method) => method switch
    {
        InvoicePaymentMethod.Cash => "cash",
        InvoicePaymentMethod.Card => "card",
        InvoicePaymentMethod.SinpeMovil => "sinpe-movil",
        InvoicePaymentMethod.BankTransfer => "bank-transfer",
        InvoicePaymentMethod.Credit => "credit",
        InvoicePaymentMethod.Mixed => "mixed",
        _ => "cash"
    };

    public static string InvoiceStatusCode(InvoiceStatus status) => status switch
    {
        InvoiceStatus.Accepted => "accepted",
        InvoiceStatus.Pending => "pending",
        InvoiceStatus.Voided => "voided",
        _ => "accepted"
    };
}
