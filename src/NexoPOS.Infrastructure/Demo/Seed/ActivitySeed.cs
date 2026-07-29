using NexoPOS.Domain.Activity;
using static NexoPOS.Infrastructure.Demo.Seed.BranchSeed;
using static NexoPOS.Infrastructure.Demo.Seed.MobileKitSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Actividad reciente de ejemplo. Cada evento conserva usuario, rol, sucursal o
/// botiquín, fecha, referencia y motivo para trazabilidad, tal como se exige
/// aunque todavía no exista autenticación real.
/// </summary>
internal static class ActivitySeed
{
    private sealed record Seed(
        string Id, ActivityType Type, string UserName, string UserRole, string BranchId,
        string? MobileKitId, int MinutesAgo, string Reference, string Reason, decimal? Amount);

    private static readonly Seed[] Seeds =
    [
        new("ev-01", ActivityType.InvoiceIssued, "Kimberly Solano", "Cajera", SanJoseId, null, 18, "TE-SJ-0001", "Venta de mostrador", 8_900m),
        new("ev-02", ActivityType.FractionalSale, "Jefferson Rojas", "Administrador general", SanJoseId, null, 150, "TE-SJ-0005", "Venta de anestesia por fracción (10 ml)", 1_800m),
        new("ev-03", ActivityType.ServiceBilled, "Jefferson Rojas", "Administrador general", SanJoseId, null, 42, "FE-SJ-0002", "Limpieza dental facturada", 65_000m),
        new("ev-04", ActivityType.TransferToMobileKit, "María Fernández", "Veterinaria", SanJoseId, Kit01Id, 120, "TRK-000031", "Reposición de suero para ruta del día", null),
        new("ev-05", ActivityType.PurchaseReception, "Ana Villalobos", "Veterinaria", CartagoId, null, 260, "OC-002045", "Recepción de pedido a Distribuidora Veterinaria CR", 845_000m),
        new("ev-06", ActivityType.InventoryAdjustment, "Carlos Jiménez", "Veterinario", HerediaId, null, 190, "AJU-000318", "Ajuste autorizado por conteo físico mensual", null),
        new("ev-07", ActivityType.ReceivablePayment, "Fabiola Méndez", "Cajera", HerediaId, null, 95, "CXC-000112", "Abono de cliente a cuenta por cobrar", 45_000m),
        new("ev-08", ActivityType.CashClosing, "Warner Solís", "Cajero", CartagoId, null, 400, "CJ-000077", "Cierre de caja del turno matutino", 312_500m),
        new("ev-09", ActivityType.TransferSent, "Jefferson Rojas", "Administrador general", SanJoseId, null, 300, "TRF-000913", "Transferencia de alimento hacia Heredia", null),
        new("ev-10", ActivityType.TransferReceived, "Carlos Jiménez", "Veterinario", HerediaId, null, 280, "TRF-000913", "Recepción de transferencia entre sucursales", null)
    ];

    public static IReadOnlyList<ActivityEvent> Build(DateTimeOffset now) => Seeds
        .Select(s => new ActivityEvent
        {
            Id = s.Id,
            Type = s.Type,
            UserName = s.UserName,
            UserRole = s.UserRole,
            BranchId = s.BranchId,
            MobileKitId = s.MobileKitId,
            Timestamp = now.AddMinutes(-s.MinutesAgo),
            Reference = s.Reference,
            Reason = s.Reason,
            Amount = s.Amount
        })
        .ToList();
}
