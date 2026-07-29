namespace NexoPOS.Domain.MobileKits;

/// <summary>Existencias de un producto dentro de un botiquín móvil, en la unidad base del producto.</summary>
public sealed class MobileKitStock
{
    public required string MobileKitId { get; init; }
    public required string ProductId { get; init; }
    public decimal Quantity { get; init; }
}
