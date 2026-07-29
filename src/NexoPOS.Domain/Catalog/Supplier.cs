namespace NexoPOS.Domain.Catalog;

/// <summary>Proveedor de productos para las órdenes de compra sugeridas.</summary>
public sealed class Supplier
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string ContactPhone { get; init; }
}
