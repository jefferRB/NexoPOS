using NexoPOS.Domain.Billing;
using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Services;
using static NexoPOS.Infrastructure.Demo.Seed.BranchSeed;
using static NexoPOS.Infrastructure.Demo.Seed.ProductSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Comprobantes de demostración: una muestra reciente por sucursal (no la
/// totalidad de los tiquetes del día, igual que un listado real paginado). Cada
/// comprobante es autoconsistente: <see cref="DemoInvoice.Total"/> es siempre la
/// suma de sus líneas.
/// </summary>
internal static class InvoiceSeed
{
    private sealed record Line(string? ProductId, string? ServiceId, decimal Quantity);

    private sealed record Blueprint(
        string BranchId, int MinutesAgo, string Customer, string IssuedBy,
        InvoiceDocumentType Type, InvoicePaymentMethod PaymentMethod, InvoiceStatus Status,
        Line[] Lines);

    private static readonly Blueprint[] Blueprints =
    [
        // Veterinaria San José
        new(SanJoseId, 18, "Cliente de mostrador", "Kimberly Solano", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(DewormerId, null, 1m)]),
        new(SanJoseId, 42, "Familia Araya Solano", "Jefferson Rojas", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Card, InvoiceStatus.Accepted,
            [new Line(null, "svc-02", 1m)]),
        new(SanJoseId, 75, "Ricardo Vindas", "Kimberly Solano", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.SinpeMovil, InvoiceStatus.Accepted,
            [new Line(SyringeId, null, 2m), new Line(GlovesId, null, 1m)]),
        new(SanJoseId, 110, "Cliente de mostrador", "Dr. Luis Araya", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(AdultFoodId, null, 5m)]),
        new(SanJoseId, 150, "Marta Chinchilla", "Jefferson Rojas", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Pending,
            [new Line(AnesthesiaId, null, 10m)]),
        new(SanJoseId, 200, "Familia Mora Vega", "Kimberly Solano", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.BankTransfer, InvoiceStatus.Accepted,
            [new Line(null, "svc-04", 1m), new Line(DewormerId, null, 1m)]),
        new(SanJoseId, 260, "Cliente de mostrador", "Dr. Luis Araya", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Voided,
            [new Line(ShampooId, null, 1m)]),
        new(SanJoseId, 320, "Esteban Rojas", "Kimberly Solano", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Accepted,
            [new Line(EarCleanerId, null, 1m), new Line(GlovesId, null, 2m)]),
        new(SanJoseId, 400, "Diego Salas", "Jefferson Rojas", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Credit, InvoiceStatus.Accepted,
            [new Line(null, "svc-01", 1m)]),

        // Veterinaria Heredia
        new(HerediaId, 22, "Cliente de mostrador", "Fabiola Méndez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(SyringeId, null, 3m)]),
        new(HerediaId, 55, "Familia Castro", "Carlos Jiménez", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Card, InvoiceStatus.Accepted,
            [new Line(null, "svc-03", 1m)]),
        new(HerediaId, 90, "Paola Jiménez", "Fabiola Méndez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.SinpeMovil, InvoiceStatus.Accepted,
            [new Line(RenalFoodId, null, 2.5m)]),
        new(HerediaId, 130, "Cliente de mostrador", "Carlos Jiménez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(SuturesId, null, 1m), new Line(GlovesId, null, 1m)]),
        new(HerediaId, 175, "Ricardo Vindas", "Fabiola Méndez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Pending,
            [new Line(FelineVaccineId, null, 1m)]),
        new(HerediaId, 230, "Marta Chinchilla", "Carlos Jiménez", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Mixed, InvoiceStatus.Accepted,
            [new Line(null, "svc-04", 1m)]),
        new(HerediaId, 300, "Cliente de mostrador", "Fabiola Méndez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(DewormerId, null, 1m)]),
        new(HerediaId, 360, "Familia Araya Solano", "Carlos Jiménez", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Voided,
            [new Line(AdultFoodId, null, 3m)]),
        new(HerediaId, 420, "Esteban Rojas", "Fabiola Méndez", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Credit, InvoiceStatus.Accepted,
            [new Line(null, "svc-01", 1m), new Line(DewormerId, null, 1m)]),

        // Veterinaria Cartago
        new(CartagoId, 20, "Cliente de mostrador", "Warner Solís", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(SyringeId, null, 1m)]),
        new(CartagoId, 60, "Familia Mora Vega", "Ana Villalobos", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Card, InvoiceStatus.Accepted,
            [new Line(null, "svc-02", 1m)]),
        new(CartagoId, 95, "Diego Salas", "Warner Solís", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.SinpeMovil, InvoiceStatus.Accepted,
            [new Line(SuturesId, null, 2m)]),
        new(CartagoId, 140, "Cliente de mostrador", "Ana Villalobos", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Pending,
            [new Line(EarCleanerId, null, 1m)]),
        new(CartagoId, 190, "Paola Jiménez", "Warner Solís", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Accepted,
            [new Line(FelineVaccineId, null, 1m), new Line(SyringeId, null, 1m)]),
        new(CartagoId, 240, "Familia Castro", "Ana Villalobos", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Mixed, InvoiceStatus.Accepted,
            [new Line(null, "svc-03", 1m)]),
        new(CartagoId, 290, "Cliente de mostrador", "Warner Solís", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Cash, InvoiceStatus.Accepted,
            [new Line(DewormerId, null, 1m)]),
        new(CartagoId, 340, "Ricardo Vindas", "Ana Villalobos", InvoiceDocumentType.ElectronicTicket, InvoicePaymentMethod.Card, InvoiceStatus.Voided,
            [new Line(ShampooId, null, 1m)]),
        new(CartagoId, 400, "Esteban Rojas", "Warner Solís", InvoiceDocumentType.ElectronicInvoice, InvoicePaymentMethod.Credit, InvoiceStatus.Accepted,
            [new Line(null, "svc-01", 1m)])
    ];

    private static readonly IReadOnlyDictionary<string, string> BranchCodes = new Dictionary<string, string>
    {
        [SanJoseId] = "SJ",
        [HerediaId] = "HD",
        [CartagoId] = "CT"
    };

    public static IReadOnlyList<DemoInvoice> Build(
        IReadOnlyDictionary<string, Product> productsById,
        IReadOnlyDictionary<string, ServiceDefinition> servicesById,
        DateTimeOffset now)
    {
        var invoices = new List<DemoInvoice>(Blueprints.Length);
        var counters = new Dictionary<string, int>();

        foreach (var blueprint in Blueprints)
        {
            var lines = new List<DemoInvoiceLine>(blueprint.Lines.Length);
            foreach (var line in blueprint.Lines)
            {
                if (line.ProductId is not null)
                {
                    var product = productsById[line.ProductId];
                    lines.Add(new DemoInvoiceLine
                    {
                        Description = product.Name,
                        Quantity = line.Quantity,
                        Unit = product.BaseUnit,
                        UnitPrice = product.UnitPrice,
                        LineTotal = decimal.Round(product.UnitPrice * line.Quantity, 2)
                    });
                }
                else
                {
                    var service = servicesById[line.ServiceId!];
                    lines.Add(new DemoInvoiceLine
                    {
                        Description = service.Name,
                        Quantity = line.Quantity,
                        Unit = MeasurementUnit.Unit,
                        UnitPrice = service.Price,
                        LineTotal = decimal.Round(service.Price * line.Quantity, 2)
                    });
                }
            }

            var code = BranchCodes[blueprint.BranchId];
            var prefix = blueprint.Type == InvoiceDocumentType.ElectronicInvoice ? "FE" : "TE";
            var index = counters.GetValueOrDefault(blueprint.BranchId, 0) + 1;
            counters[blueprint.BranchId] = index;

            invoices.Add(new DemoInvoice
            {
                Id = $"inv-{code.ToLowerInvariant()}-{index:D3}",
                Number = $"{prefix}-{code}-{index:D4}",
                IssuedAt = now.AddMinutes(-blueprint.MinutesAgo),
                BranchId = blueprint.BranchId,
                CustomerName = blueprint.Customer,
                IssuedBy = blueprint.IssuedBy,
                Type = blueprint.Type,
                PaymentMethod = blueprint.PaymentMethod,
                Status = blueprint.Status,
                Total = lines.Sum(l => l.LineTotal),
                Lines = lines
            });
        }

        return invoices;
    }
}
