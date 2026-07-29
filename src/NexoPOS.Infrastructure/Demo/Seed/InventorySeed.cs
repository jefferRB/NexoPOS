using NexoPOS.Domain.Inventory;
using static NexoPOS.Infrastructure.Demo.Seed.BranchSeed;
using static NexoPOS.Infrastructure.Demo.Seed.ProductSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Existencias por sucursal, en la unidad base de cada producto. El total
/// consolidado (sucursales + botiquines, ver <see cref="MobileKitSeed"/>) debe
/// coincidir con la suma de estas cantidades: por ejemplo, la anestesia queda en
/// San José 420 ml, Heredia 260 ml, Cartago 180 ml, botiquines 60 ml = 920 ml.
/// </summary>
internal static class InventorySeed
{
    public static readonly IReadOnlyList<InventoryStock> Stock = Build();

    private static IReadOnlyList<InventoryStock> Build()
    {
        // producto -> (San José, Heredia, Cartago)
        var quantities = new (string ProductId, decimal SanJose, decimal Heredia, decimal Cartago)[]
        {
            (AnesthesiaId, 420m, 260m, 180m),      // Heredia bajo (min 300)
            (SalineId, 850m, 650m, 180m),          // Cartago bajo (min 600)
            (AdultFoodId, 180m, 100m, 62.5m),      // decimales en kg
            (RenalFoodId, 25m, 12.5m, 0m),         // Heredia bajo, Cartago agotado (min 20)
            (CanineVaccineId, 40m, 18m, 0m),       // Cartago agotado (min 15)
            (FelineVaccineId, 22m, 14m, 9m),       // Cartago bajo (min 12)
            (SyringeId, 500m, 320m, 210m),
            (GlovesId, 600m, 380m, 240m),
            (ShampooId, 18m, 9m, 14m),             // Heredia bajo (min 10)
            (DewormerId, 45m, 28m, 17m),           // Cartago bajo (min 20)
            (SuturesId, 60m, 33m, 22m),            // Cartago bajo (min 25)
            (EarCleanerId, 20m, 11m, 15m)          // Heredia bajo (min 12)
        };

        var result = new List<InventoryStock>(quantities.Length * 3);
        foreach (var q in quantities)
        {
            result.Add(new InventoryStock { ProductId = q.ProductId, BranchId = SanJoseId, Quantity = q.SanJose });
            result.Add(new InventoryStock { ProductId = q.ProductId, BranchId = HerediaId, Quantity = q.Heredia });
            result.Add(new InventoryStock { ProductId = q.ProductId, BranchId = CartagoId, Quantity = q.Cartago });
        }

        return result;
    }
}
