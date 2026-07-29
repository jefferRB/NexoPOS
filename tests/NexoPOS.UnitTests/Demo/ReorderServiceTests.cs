using NexoPOS.Application.Demo;
using NexoPOS.Application.Demo.Abstractions;

namespace NexoPOS.UnitTests.Demo;

public sealed class ReorderServiceTests
{
    private static IReorderService CreateService() => new ReorderService(new FakeDemoOperationsRepository());

    [Fact]
    public async Task GetReorderSuggestions_semanal_solo_incluye_productos_con_historial_de_venta()
    {
        var response = await CreateService().GetReorderSuggestionsAsync(ReorderBasis.Weekly);

        // pB no tiene venta promedio registrada; no debería aparecer.
        var order = Assert.Single(response.SupplierOrders);
        var item = Assert.Single(order.Items);
        Assert.Equal("pA", item.ProductId);
        Assert.True(item.CoverageDays < 14m);
    }

    [Fact]
    public async Task GetReorderSuggestions_mensual_devuelve_resultados_consistentes_con_lo_semanal()
    {
        var weekly = await CreateService().GetReorderSuggestionsAsync(ReorderBasis.Weekly);
        var monthly = await CreateService().GetReorderSuggestionsAsync(ReorderBasis.Monthly);

        Assert.Equal(weekly.Indicators.ProductsToReorder, monthly.Indicators.ProductsToReorder);
        Assert.Equal("weekly", weekly.Basis);
        Assert.Equal("monthly", monthly.Basis);
    }

    [Fact]
    public async Task GetReorderSuggestions_agrupa_por_proveedor_con_valor_estimado_positivo()
    {
        var response = await CreateService().GetReorderSuggestionsAsync(ReorderBasis.Weekly);

        var order = Assert.Single(response.SupplierOrders);
        Assert.Equal(FakeDemoOperationsRepository.SupplierId, order.SupplierId);
        Assert.True(order.EstimatedValue > 0);
        Assert.Equal(order.Items.Sum(i => i.SuggestedQuantity * 100m), order.EstimatedValue); // UnitPrice de pA = 100
    }
}
