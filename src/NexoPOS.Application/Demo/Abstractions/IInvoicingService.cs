using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>Comprobantes de demostración emitidos por las sucursales.</summary>
public interface IInvoicingService
{
    Task<InvoiceListResponseDto> GetInvoicesAsync(CancellationToken cancellationToken = default);

    /// <summary>Devuelve el detalle del comprobante, o <c>null</c> si no existe.</summary>
    Task<InvoiceDetailDto?> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default);
}
