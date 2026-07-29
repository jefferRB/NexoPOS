namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Línea de un comprobante: un producto o un servicio facturado.</summary>
public sealed record InvoiceLineDto(
    string Description,
    decimal Quantity,
    string Unit,
    decimal UnitPrice,
    decimal LineTotal);

/// <summary>Renglón de la lista de comprobantes (vista de solo lectura).</summary>
public sealed record InvoiceSummaryDto(
    string Id,
    string Number,
    DateTimeOffset IssuedAt,
    string BranchId,
    string BranchName,
    string CustomerName,
    string IssuedBy,
    string Type,
    string PaymentMethod,
    decimal Total,
    string Status);

/// <summary>Detalle completo de un comprobante para el modal.</summary>
public sealed record InvoiceDetailDto(
    InvoiceSummaryDto Summary,
    IReadOnlyList<InvoiceLineDto> Lines);

/// <summary>Indicadores de la pantalla de facturación.</summary>
public sealed record InvoiceIndicatorsDto(
    int IssuedToday,
    int Accepted,
    int Pending,
    int Voided,
    decimal AverageTicket);

/// <summary>Respuesta de la lista de comprobantes: indicadores + muestra reciente.</summary>
public sealed record InvoiceListResponseDto(
    InvoiceIndicatorsDto Indicators,
    IReadOnlyList<InvoiceSummaryDto> Invoices);
