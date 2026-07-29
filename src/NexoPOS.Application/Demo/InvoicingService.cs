using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;
using NexoPOS.Domain.Billing;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Expone una vista de solo lectura de los comprobantes de demostración. No
/// representa una integración fiscal real: no hay clave, XML ni consecutivo
/// oficial de Hacienda.
/// </summary>
public sealed class InvoicingService(IDemoOperationsRepository repository) : IInvoicingService
{
    public async Task<InvoiceListResponseDto> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        var invoices = await repository.GetInvoicesAsync(cancellationToken);
        var branches = await repository.GetBranchesAsync(cancellationToken);
        var branchNames = branches.ToDictionary(b => b.Id, b => b.Name);

        var summaries = invoices
            .OrderByDescending(i => i.IssuedAt)
            .Select(i => MapSummary(i, branchNames))
            .ToList();

        var accepted = summaries.Count(i => i.Status == "accepted");
        var pending = summaries.Count(i => i.Status == "pending");
        var voided = summaries.Count(i => i.Status == "voided");
        var averageTicket = summaries.Count == 0 ? 0m : decimal.Round(summaries.Average(i => i.Total), 2);

        var indicators = new InvoiceIndicatorsDto(
            IssuedToday: summaries.Count,
            Accepted: accepted,
            Pending: pending,
            Voided: voided,
            AverageTicket: averageTicket);

        return new InvoiceListResponseDto(indicators, summaries);
    }

    public async Task<InvoiceDetailDto?> GetInvoiceAsync(string invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await repository.GetInvoiceAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return null;
        }

        var branches = await repository.GetBranchesAsync(cancellationToken);
        var branchNames = branches.ToDictionary(b => b.Id, b => b.Name);

        var lines = invoice.Lines
            .Select(l => new InvoiceLineDto(
                Description: l.Description,
                Quantity: l.Quantity,
                Unit: DemoPresentation.UnitCode(l.Unit),
                UnitPrice: l.UnitPrice,
                LineTotal: l.LineTotal))
            .ToList();

        return new InvoiceDetailDto(MapSummary(invoice, branchNames), lines);
    }

    private static InvoiceSummaryDto MapSummary(DemoInvoice invoice, IReadOnlyDictionary<string, string> branchNames) =>
        new(
            Id: invoice.Id,
            Number: invoice.Number,
            IssuedAt: invoice.IssuedAt,
            BranchId: invoice.BranchId,
            BranchName: branchNames.GetValueOrDefault(invoice.BranchId, invoice.BranchId),
            CustomerName: invoice.CustomerName,
            IssuedBy: invoice.IssuedBy,
            Type: DemoPresentation.InvoiceTypeCode(invoice.Type),
            PaymentMethod: DemoPresentation.PaymentMethodCode(invoice.PaymentMethod),
            Total: invoice.Total,
            Status: DemoPresentation.InvoiceStatusCode(invoice.Status));
}
