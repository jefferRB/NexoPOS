namespace NexoPOS.Domain.Inventory;

/// <summary>
/// Existencias de un producto en una sucursal concreta, en la unidad base del
/// producto. Es <see cref="decimal"/> porque los productos fraccionables se
/// administran por fracción (mililitros, kilogramos).
/// </summary>
public sealed class InventoryStock
{
    public required string ProductId { get; init; }
    public required string BranchId { get; init; }
    public decimal Quantity { get; init; }
}
