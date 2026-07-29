using NexoPOS.Domain.Catalog;

namespace NexoPOS.Domain.Services;

/// <summary>
/// Un componente de un servicio compuesto. Si <see cref="ProductId"/> no es
/// nulo, el componente descuenta inventario real al facturar el servicio. Si
/// <see cref="DurationMinutes"/> no es nulo, representa tiempo de veterinario en
/// lugar de un producto.
/// </summary>
public sealed class ServiceComponent
{
    public string? ProductId { get; init; }
    public required string Label { get; init; }
    public decimal? Quantity { get; init; }
    public MeasurementUnit? Unit { get; init; }
    public int? DurationMinutes { get; init; }
}
