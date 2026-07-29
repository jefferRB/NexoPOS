using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.IntegrationTests.Demo;

/// <summary>
/// Pruebas de integración de los endpoints de demostración usando el API real
/// con la Infraestructura en memoria (WebApplicationFactory).
/// </summary>
public sealed class DemoEndpointsTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient CreateClient() => factory.CreateClient();

    [Fact]
    public async Task Get_overview_responde_200_con_tres_sucursales()
    {
        var response = await CreateClient().GetAsync("/api/demo/overview");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var overview = await response.Content.ReadFromJsonAsync<OverviewDto>();
        Assert.NotNull(overview);
        Assert.Equal(3, overview!.Branches.Count);
    }

    [Fact]
    public async Task Get_inventory_responde_200_con_productos()
    {
        var response = await CreateClient().GetAsync("/api/demo/inventory");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var inventory = await response.Content.ReadFromJsonAsync<InventoryDto>();
        Assert.NotNull(inventory);
        Assert.True(inventory!.Items.Count >= 10, "Debe haber al menos 10 productos de demostración.");
    }

    [Fact]
    public async Task Get_services_responde_200_con_servicios()
    {
        var response = await CreateClient().GetAsync("/api/demo/services");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var services = await response.Content.ReadFromJsonAsync<List<ServiceDto>>();
        Assert.NotNull(services);
        Assert.NotEmpty(services!);
    }

    [Fact]
    public async Task Get_mobile_kits_responde_200_con_tres_botiquines()
    {
        var response = await CreateClient().GetAsync("/api/demo/mobile-kits");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var kits = await response.Content.ReadFromJsonAsync<List<MobileKitDto>>();
        Assert.NotNull(kits);
        Assert.Equal(3, kits!.Count);
    }

    [Fact]
    public async Task Get_invoices_responde_200_con_comprobantes()
    {
        var response = await CreateClient().GetAsync("/api/demo/invoices");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var invoices = await response.Content.ReadFromJsonAsync<InvoiceListResponseDto>();
        Assert.NotNull(invoices);
        Assert.NotEmpty(invoices!.Invoices);
    }

    [Fact]
    public async Task Get_reorder_semanal_responde_200()
    {
        var response = await CreateClient().GetAsync("/api/demo/reorder?basis=weekly");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var reorder = await response.Content.ReadFromJsonAsync<ReorderResponseDto>();
        Assert.NotNull(reorder);
        Assert.Equal("weekly", reorder!.Basis);
    }

    [Fact]
    public async Task Get_reorder_mensual_responde_200()
    {
        var response = await CreateClient().GetAsync("/api/demo/reorder?basis=monthly");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var reorder = await response.Content.ReadFromJsonAsync<ReorderResponseDto>();
        Assert.NotNull(reorder);
        Assert.Equal("monthly", reorder!.Basis);
    }

    [Fact]
    public async Task Get_branch_valida_responde_200()
    {
        var response = await CreateClient().GetAsync("/api/demo/branches/sanjose");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var detail = await response.Content.ReadFromJsonAsync<BranchDetailDto>();
        Assert.NotNull(detail);
        Assert.Equal("sanjose", detail!.Branch.Id);
    }

    [Fact]
    public async Task Get_branch_inexistente_responde_404()
    {
        var response = await CreateClient().GetAsync("/api/demo/branches/no-existe");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_invoice_inexistente_responde_404()
    {
        var response = await CreateClient().GetAsync("/api/demo/invoices/no-existe");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
