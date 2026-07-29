using NexoPOS.Domain.MobileKits;
using static NexoPOS.Infrastructure.Demo.Seed.BranchSeed;
using static NexoPOS.Infrastructure.Demo.Seed.ProductSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>Botiquines móviles asignados a veterinarios de visita a domicilio.</summary>
internal static class MobileKitSeed
{
    public const string Kit01Id = "kit-01";
    public const string Kit02Id = "kit-02";
    public const string Kit03Id = "kit-03";

    public static IReadOnlyList<MobileKit> Build(DateTimeOffset now) =>
    [
        new MobileKit
        {
            Id = Kit01Id, Name = "Botiquín Móvil 01", AssignedTo = "Dra. María Fernández",
            HomeBranchId = SanJoseId, Status = MobileKitStatus.OnRoute,
            LastTransferAt = now.AddHours(-2), LastConsumptionAt = now.AddMinutes(-25),
            Alerts = []
        },
        new MobileKit
        {
            Id = Kit02Id, Name = "Botiquín Móvil 02", AssignedTo = "Dr. Carlos Jiménez",
            HomeBranchId = HerediaId, Status = MobileKitStatus.Available,
            LastTransferAt = now.AddHours(-20), LastConsumptionAt = now.AddHours(-3),
            Alerts = []
        },
        new MobileKit
        {
            Id = Kit03Id, Name = "Botiquín Móvil 03", AssignedTo = "Dra. Ana Villalobos",
            HomeBranchId = CartagoId, Status = MobileKitStatus.NeedsReview,
            LastTransferAt = now.AddHours(-5), LastConsumptionAt = now.AddHours(-1),
            Alerts = ["Diferencia de 1 jeringa pendiente de justificar"]
        }
    ];

    // producto -> (kit-01, kit-02, kit-03). Cada fila suma la cantidad total en botiquines de InventorySeed/ProductSeed.
    private static readonly (string ProductId, decimal Kit01, decimal Kit02, decimal Kit03)[] Quantities =
    [
        (AnesthesiaId, 30m, 20m, 10m),
        (SalineId, 60m, 50m, 30m),
        (CanineVaccineId, 2m, 2m, 2m),
        (FelineVaccineId, 1m, 1m, 1m),
        (SyringeId, 20m, 15m, 10m),
        (GlovesId, 25m, 20m, 15m),
        (DewormerId, 3m, 2m, 1m),
        (SuturesId, 3m, 3m, 2m)
    ];

    public static IReadOnlyList<MobileKitStock> BuildStock()
    {
        var result = new List<MobileKitStock>(Quantities.Length * 3);
        foreach (var q in Quantities)
        {
            result.Add(new MobileKitStock { MobileKitId = Kit01Id, ProductId = q.ProductId, Quantity = q.Kit01 });
            result.Add(new MobileKitStock { MobileKitId = Kit02Id, ProductId = q.ProductId, Quantity = q.Kit02 });
            result.Add(new MobileKitStock { MobileKitId = Kit03Id, ProductId = q.ProductId, Quantity = q.Kit03 });
        }

        return result;
    }
}
