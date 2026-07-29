using NexoPOS.Domain.Branches;
using static NexoPOS.Infrastructure.Demo.Seed.BranchSeed;
using static NexoPOS.Infrastructure.Demo.Seed.ProductSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Desempeño ilustrativo de los últimos 7 días por sucursal. El último día
/// (hoy) coincide exactamente con <see cref="Branch.TicketsToday"/> y
/// <see cref="Branch.SalesToday"/> de <see cref="BranchSeed"/> para que el
/// dashboard y el detalle de sucursal nunca se contradigan.
/// </summary>
internal static class PerformanceSeed
{
    // (diasAtras, tickets, ventas) — día 0 = hoy, debe igualar BranchSeed.
    private static readonly (int DaysAgo, int Tickets, decimal Sales)[] SanJose =
    [
        (6, 55, 1_150_000m), (5, 60, 1_225_000m), (4, 58, 1_190_500m),
        (3, 64, 1_340_800m), (2, 61, 1_275_200m), (1, 66, 1_390_000m), (0, 68, 1_420_350m)
    ];

    private static readonly (int DaysAgo, int Tickets, decimal Sales)[] Heredia =
    [
        (6, 46, 780_000m), (5, 50, 845_000m), (4, 48, 810_500m),
        (3, 53, 905_300m), (2, 51, 872_000m), (1, 55, 950_000m), (0, 57, 986_800m)
    ];

    private static readonly (int DaysAgo, int Tickets, decimal Sales)[] Cartago =
    [
        (6, 42, 640_000m), (5, 45, 690_000m), (4, 44, 675_500m),
        (3, 49, 758_300m), (2, 47, 725_000m), (1, 51, 795_000m), (0, 53, 821_450m)
    ];

    public static IReadOnlyList<BranchDailyPerformance> Build(DateOnly today)
    {
        var result = new List<BranchDailyPerformance>();
        AddSeries(result, SanJoseId, SanJose, today);
        AddSeries(result, HerediaId, Heredia, today);
        AddSeries(result, CartagoId, Cartago, today);
        return result;
    }

    private static void AddSeries(
        List<BranchDailyPerformance> result, string branchId,
        (int DaysAgo, int Tickets, decimal Sales)[] series, DateOnly today)
    {
        foreach (var (daysAgo, tickets, sales) in series)
        {
            result.Add(new BranchDailyPerformance
            {
                BranchId = branchId,
                Date = today.AddDays(-daysAgo),
                Tickets = tickets,
                Sales = sales
            });
        }
    }

    public static IReadOnlyList<BranchTopProduct> TopProducts =>
    [
        new BranchTopProduct { BranchId = SanJoseId, ProductId = SyringeId, QuantitySold = 312m },
        new BranchTopProduct { BranchId = SanJoseId, ProductId = GlovesId, QuantitySold = 298m },
        new BranchTopProduct { BranchId = SanJoseId, ProductId = DewormerId, QuantitySold = 86m },
        new BranchTopProduct { BranchId = SanJoseId, ProductId = CanineVaccineId, QuantitySold = 34m },

        new BranchTopProduct { BranchId = HerediaId, ProductId = RenalFoodId, QuantitySold = 145m },
        new BranchTopProduct { BranchId = HerediaId, ProductId = SyringeId, QuantitySold = 204m },
        new BranchTopProduct { BranchId = HerediaId, ProductId = SuturesId, QuantitySold = 58m },

        new BranchTopProduct { BranchId = CartagoId, ProductId = FelineVaccineId, QuantitySold = 28m },
        new BranchTopProduct { BranchId = CartagoId, ProductId = GlovesId, QuantitySold = 176m },
        new BranchTopProduct { BranchId = CartagoId, ProductId = ShampooId, QuantitySold = 39m }
    ];
}
