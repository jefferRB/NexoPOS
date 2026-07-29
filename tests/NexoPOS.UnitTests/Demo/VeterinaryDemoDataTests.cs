using NexoPOS.Application.Demo;
using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Infrastructure.Demo;

namespace NexoPOS.UnitTests.Demo;

/// <summary>
/// Valida que los datos de demostración reales de Grupo Veterinario Demo (no el
/// repositorio falso) cumplan las invariantes exigidas: totales coherentes,
/// fracciones exactas, servicios que referencian productos existentes y
/// comprobantes/botiquines vinculados a sucursales válidas.
/// </summary>
public sealed class VeterinaryDemoDataTests
{
    private static (IBusinessOverviewService Overview, IServicesCatalogService Services, IMobileKitsService MobileKits, IInvoicingService Invoicing, IReorderService Reorder) CreateServices()
    {
        var repository = new DemoOperationsRepository();
        var reorder = new ReorderService(repository);
        var overview = new BusinessOverviewService(repository, reorder);
        var services = new ServicesCatalogService(repository);
        var mobileKits = new MobileKitsService(repository);
        var invoicing = new InvoicingService(repository);
        return (overview, services, mobileKits, invoicing, reorder);
    }

    [Fact]
    public async Task Overview_total_de_tickets_es_178()
    {
        var (overview, _, _, _, _) = CreateServices();

        var result = await overview.GetOverviewAsync();

        Assert.Equal(178, result.Indicators.TicketsToday);
        Assert.Equal(result.Branches.Sum(b => b.TicketsToday), result.Indicators.TicketsToday);
    }

    [Fact]
    public async Task Overview_total_de_ventas_es_3_228_600_colones()
    {
        var (overview, _, _, _, _) = CreateServices();

        var result = await overview.GetOverviewAsync();

        Assert.Equal(3_228_600m, result.Indicators.SalesToday);
        Assert.Equal(result.Branches.Sum(b => b.SalesToday), result.Indicators.SalesToday);
    }

    [Fact]
    public async Task Inventario_de_anestesia_suma_sucursales_y_botiquines_en_la_misma_unidad()
    {
        var (overview, _, _, _, _) = CreateServices();

        var inventory = await overview.GetInventoryAsync();
        var anesthesia = inventory.Items.Single(i => i.InternalCode == "MED-001");

        Assert.Equal("ml", anesthesia.Unit);
        Assert.Equal(420m, anesthesia.StockByBranch["sanjose"]);
        Assert.Equal(260m, anesthesia.StockByBranch["heredia"]);
        Assert.Equal(180m, anesthesia.StockByBranch["cartago"]);
        Assert.Equal(60m, anesthesia.MobileKitsStock);
        Assert.Equal(920m, anesthesia.Total);
    }

    [Fact]
    public async Task Inventario_fraccionable_conserva_decimales_exactos()
    {
        var (overview, _, _, _, _) = CreateServices();

        var inventory = await overview.GetInventoryAsync();
        var renalFood = inventory.Items.Single(i => i.InternalCode == "ALI-002");

        Assert.True(renalFood.IsFractionable);
        Assert.Equal(12.5m, renalFood.StockByBranch["heredia"]);
    }

    [Fact]
    public async Task Servicios_compuestos_solo_referencian_productos_que_existen_en_el_catalogo()
    {
        var (overview, services, _, _, _) = CreateServices();

        var catalog = await services.GetServicesAsync();
        var inventory = await overview.GetInventoryAsync();
        var validProductIds = inventory.Items.Select(i => i.ProductId).ToHashSet();

        var componentsWithProduct = catalog.SelectMany(s => s.Components).Where(c => c.ProductId is not null).ToList();
        Assert.NotEmpty(componentsWithProduct);
        Assert.All(componentsWithProduct, c => Assert.Contains(c.ProductId!, validProductIds));
    }

    [Fact]
    public async Task Reposicion_semanal_devuelve_sugerencias_con_cobertura_menor_a_catorce_dias()
    {
        var (_, _, _, _, reorder) = CreateServices();

        var response = await reorder.GetReorderSuggestionsAsync(ReorderBasis.Weekly);
        var allItems = response.SupplierOrders.SelectMany(o => o.Items).ToList();

        Assert.NotEmpty(allItems);
        Assert.All(allItems, i => Assert.True(i.CoverageDays < 14m));
    }

    [Fact]
    public async Task Reposicion_mensual_tambien_devuelve_sugerencias_validas()
    {
        var (_, _, _, _, reorder) = CreateServices();

        var response = await reorder.GetReorderSuggestionsAsync(ReorderBasis.Monthly);
        var allItems = response.SupplierOrders.SelectMany(o => o.Items).ToList();

        Assert.NotEmpty(allItems);
        Assert.All(allItems, i => Assert.True(i.CoverageDays < 14m));
    }

    [Fact]
    public async Task Botiquines_moviles_estan_vinculados_a_sucursales_validas()
    {
        var (overview, _, mobileKitsService, _, _) = CreateServices();

        var kits = await mobileKitsService.GetMobileKitsAsync();
        Assert.Equal(3, kits.Count);

        foreach (var kit in kits)
        {
            var branch = await overview.GetBranchDetailAsync(kit.HomeBranchId);
            Assert.NotNull(branch);
            Assert.NotEmpty(kit.Stock);
        }
    }

    [Fact]
    public async Task Facturas_referencian_sucursales_validas_y_sus_totales_coinciden_con_las_lineas()
    {
        var (overview, _, _, invoicing, _) = CreateServices();

        var invoiceList = await invoicing.GetInvoicesAsync();
        Assert.NotEmpty(invoiceList.Invoices);

        foreach (var summary in invoiceList.Invoices)
        {
            var branch = await overview.GetBranchDetailAsync(summary.BranchId);
            Assert.NotNull(branch);

            var detail = await invoicing.GetInvoiceAsync(summary.Id);
            Assert.NotNull(detail);
            Assert.Equal(detail!.Lines.Sum(l => l.LineTotal), detail.Summary.Total);
        }
    }

    [Fact]
    public async Task Sucursal_inexistente_devuelve_null()
    {
        var (overview, _, _, _, _) = CreateServices();

        Assert.Null(await overview.GetBranchDetailAsync("no-existe"));
    }

    [Fact]
    public async Task Comprobante_inexistente_devuelve_null()
    {
        var (_, _, _, invoicing, _) = CreateServices();

        Assert.Null(await invoicing.GetInvoiceAsync("no-existe"));
    }
}
