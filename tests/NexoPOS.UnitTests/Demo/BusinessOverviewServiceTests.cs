using NexoPOS.Application.Demo;
using NexoPOS.Application.Demo.Abstractions;

namespace NexoPOS.UnitTests.Demo;

public sealed class BusinessOverviewServiceTests
{
    private static IBusinessOverviewService CreateService()
    {
        var repository = new FakeDemoOperationsRepository();
        var reorderService = new ReorderService(repository);
        return new BusinessOverviewService(repository, reorderService);
    }

    [Fact]
    public async Task GetOverview_devuelve_tres_sucursales()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.Equal(3, overview.Branches.Count);
    }

    [Fact]
    public async Task GetOverview_total_de_tickets_coincide_con_la_suma_por_sucursal()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.Equal(45, overview.Indicators.TicketsToday);
        Assert.Equal(overview.Branches.Sum(b => b.TicketsToday), overview.Indicators.TicketsToday);
    }

    [Fact]
    public async Task GetOverview_total_de_ventas_coincide_con_la_suma_por_sucursal()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.Equal(600m, overview.Indicators.SalesToday);
        Assert.Equal(overview.Branches.Sum(b => b.SalesToday), overview.Indicators.SalesToday);
    }

    [Fact]
    public async Task GetOverview_cuentas_por_cobrar_y_pagar_coinciden_con_la_suma_por_sucursal()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.Equal(170m, overview.Indicators.ReceivablesTotal);
        Assert.Equal(110m, overview.Indicators.PayablesTotal);
    }

    [Fact]
    public async Task GetOverview_identifica_correctamente_el_stock_bajo_y_agotado_por_sucursal()
    {
        var overview = await CreateService().GetOverviewAsync();

        var b1 = overview.Branches.Single(b => b.Id == "b1");
        var b2 = overview.Branches.Single(b => b.Id == "b2");
        var b3 = overview.Branches.Single(b => b.Id == "b3");

        Assert.Equal(1, b1.LowStockCount); // pB bajo
        Assert.Equal(1, b2.LowStockCount); // pA bajo
        Assert.Equal(1, b3.LowStockCount); // pA agotado
        Assert.Equal(3, overview.Indicators.LowStockProducts);
    }

    [Fact]
    public async Task GetOverview_reposicion_sugerida_refleja_los_productos_con_baja_cobertura()
    {
        var overview = await CreateService().GetOverviewAsync();

        // Solo pA tiene historial de venta (pB no); su cobertura (63 ml / 10 ml diarios ≈ 6.3 días) requiere reposición.
        Assert.Equal(1, overview.Indicators.ReorderSuggestedCount);
    }

    [Fact]
    public async Task GetOverview_botiquines_se_asignan_a_la_sucursal_correcta()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.Equal(0, overview.Branches.Single(b => b.Id == "b1").MobileKitsCount);
        Assert.Equal(1, overview.Branches.Single(b => b.Id == "b2").MobileKitsCount);
    }

    [Fact]
    public async Task GetOverview_incluye_alertas_prioritarias_heterogeneas()
    {
        var overview = await CreateService().GetOverviewAsync();

        var categories = overview.PriorityAlerts.Select(a => a.Category).ToList();
        Assert.Contains("stock-low", categories);
        Assert.Contains("stock-out", categories);
        Assert.Contains("transfer", categories);
        Assert.Contains("reorder", categories);
    }

    [Fact]
    public async Task GetOverview_actividad_incluye_trazabilidad_completa()
    {
        var overview = await CreateService().GetOverviewAsync();

        Assert.All(overview.RecentActivity, e =>
        {
            Assert.False(string.IsNullOrWhiteSpace(e.UserName));
            Assert.False(string.IsNullOrWhiteSpace(e.UserRole));
            Assert.False(string.IsNullOrWhiteSpace(e.LocationName));
            Assert.False(string.IsNullOrWhiteSpace(e.Reason));
        });

        var kitEvent = overview.RecentActivity.Single(e => e.MobileKitId != null);
        Assert.Equal("Botiquín Z", kitEvent.LocationName);
    }

    [Fact]
    public async Task GetInventory_consolida_existencias_de_sucursales_y_botiquines_sin_mezclar_unidades()
    {
        var inventory = await CreateService().GetInventoryAsync();

        Assert.Equal(3, inventory.Branches.Count);
        Assert.Equal(2, inventory.Items.Count);

        var productA = inventory.Items.Single(i => i.InternalCode == "SKU-A");
        Assert.Equal("ml", productA.Unit);
        Assert.True(productA.IsFractionable);
        Assert.Equal(5m, productA.MobileKitsStock);
        Assert.Equal(63m, productA.Total); // 50 + 8 + 0 + 5
        Assert.Equal(productA.StockByBranch.Values.Sum() + productA.MobileKitsStock, productA.Total);

        var productB = inventory.Items.Single(i => i.InternalCode == "SKU-B");
        Assert.Equal("unit", productB.Unit);
        Assert.False(productB.IsFractionable);
        Assert.Equal(38m, productB.Total);
    }

    [Fact]
    public async Task GetBranchDetail_sucursal_valida_incluye_inventario_movimientos_y_desempeno()
    {
        var detail = await CreateService().GetBranchDetailAsync("b1");

        Assert.NotNull(detail);
        Assert.Equal("b1", detail!.Branch.Id);
        Assert.Equal(10, detail.Branch.TicketsToday);
        Assert.Equal(2, detail.Inventory.Count);
        Assert.All(detail.RecentActivity, e => Assert.Equal("b1", e.BranchId));
        Assert.Equal(2, detail.WeeklyPerformance.Count);
        Assert.Single(detail.TopProducts);
    }

    [Fact]
    public async Task GetBranchDetail_sucursal_inexistente_devuelve_null()
    {
        var detail = await CreateService().GetBranchDetailAsync("no-existe");

        Assert.Null(detail);
    }
}
