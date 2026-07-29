using Microsoft.AspNetCore.Mvc;
using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.API.Controllers;

/// <summary>
/// Endpoints de la maqueta de demostración. Todos los datos provienen del
/// repositorio en memoria; no hay base de datos ni autenticación real, y los
/// comprobantes no representan una integración fiscal real.
/// </summary>
[ApiController]
[Route("api/demo")]
public sealed class DemoController(
    IBusinessOverviewService overviewService,
    IServicesCatalogService servicesCatalogService,
    IMobileKitsService mobileKitsService,
    IInvoicingService invoicingService,
    IReorderService reorderService) : ControllerBase
{
    /// <summary>Resumen general: indicadores, sucursales, alertas prioritarias y actividad reciente.</summary>
    [HttpGet("overview")]
    [ProducesResponseType(typeof(OverviewDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OverviewDto>> GetOverview(CancellationToken cancellationToken)
    {
        var overview = await overviewService.GetOverviewAsync(cancellationToken);
        return Ok(overview);
    }

    /// <summary>Inventario consolidado de todas las sucursales y botiquines móviles.</summary>
    [HttpGet("inventory")]
    [ProducesResponseType(typeof(InventoryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InventoryDto>> GetInventory(CancellationToken cancellationToken)
    {
        var inventory = await overviewService.GetInventoryAsync(cancellationToken);
        return Ok(inventory);
    }

    /// <summary>Detalle de una sucursal. Devuelve 404 si el identificador no existe.</summary>
    [HttpGet("branches/{branchId}")]
    [ProducesResponseType(typeof(BranchDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BranchDetailDto>> GetBranch(string branchId, CancellationToken cancellationToken)
    {
        var detail = await overviewService.GetBranchDetailAsync(branchId, cancellationToken);
        if (detail is null)
        {
            return Problem(
                title: "Sucursal no encontrada",
                detail: $"No existe una sucursal con el identificador '{branchId}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(detail);
    }

    /// <summary>Detalle ampliado de un producto. Devuelve 404 si el identificador no existe.</summary>
    [HttpGet("products/{productId}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailDto>> GetProduct(string productId, CancellationToken cancellationToken)
    {
        var detail = await overviewService.GetProductDetailAsync(productId, cancellationToken);
        if (detail is null)
        {
            return Problem(
                title: "Producto no encontrado",
                detail: $"No existe un producto con el identificador '{productId}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(detail);
    }

    /// <summary>Catálogo de servicios y paquetes compuestos.</summary>
    [HttpGet("services")]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetServices(CancellationToken cancellationToken)
    {
        var services = await servicesCatalogService.GetServicesAsync(cancellationToken);
        return Ok(services);
    }

    /// <summary>Botiquines móviles asignados a veterinarios de visita a domicilio.</summary>
    [HttpGet("mobile-kits")]
    [ProducesResponseType(typeof(IReadOnlyList<MobileKitDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MobileKitDto>>> GetMobileKits(CancellationToken cancellationToken)
    {
        var kits = await mobileKitsService.GetMobileKitsAsync(cancellationToken);
        return Ok(kits);
    }

    /// <summary>
    /// Comprobantes de demostración. Vista de solo lectura: la integración y las
    /// reglas fiscales definitivas se configurarán con la información oficial de
    /// la empresa.
    /// </summary>
    [HttpGet("invoices")]
    [ProducesResponseType(typeof(InvoiceListResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InvoiceListResponseDto>> GetInvoices(CancellationToken cancellationToken)
    {
        var invoices = await invoicingService.GetInvoicesAsync(cancellationToken);
        return Ok(invoices);
    }

    /// <summary>Detalle de un comprobante. Devuelve 404 si el identificador no existe.</summary>
    [HttpGet("invoices/{invoiceId}")]
    [ProducesResponseType(typeof(InvoiceDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceDetailDto>> GetInvoice(string invoiceId, CancellationToken cancellationToken)
    {
        var invoice = await invoicingService.GetInvoiceAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Problem(
                title: "Comprobante no encontrado",
                detail: $"No existe un comprobante con el identificador '{invoiceId}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Sugerencias de reposición según el consumo promedio. <paramref name="basis"/>
    /// acepta "weekly" o "monthly"; cualquier otro valor (o su ausencia) usa "weekly".
    /// </summary>
    [HttpGet("reorder")]
    [ProducesResponseType(typeof(ReorderResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ReorderResponseDto>> GetReorderSuggestions(
        [FromQuery] string? basis, CancellationToken cancellationToken)
    {
        var reorderBasis = string.Equals(basis, "monthly", StringComparison.OrdinalIgnoreCase)
            ? ReorderBasis.Monthly
            : ReorderBasis.Weekly;

        var suggestions = await reorderService.GetReorderSuggestionsAsync(reorderBasis, cancellationToken);
        return Ok(suggestions);
    }
}
