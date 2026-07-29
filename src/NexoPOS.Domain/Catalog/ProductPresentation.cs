namespace NexoPOS.Domain.Catalog;

/// <summary>
/// Describe cómo se compra un producto fraccionable frente a cómo se administra
/// y se vende en la unidad base. Por ejemplo, un saco de 20 kg se compra como
/// una unidad pero se controla y se vende en kilogramos.
/// </summary>
public sealed class ProductPresentation
{
    public required string PurchaseUnitLabel { get; init; }
    public required decimal BaseUnitsPerPurchaseUnit { get; init; }
}
