using NexoPOS.Domain.Inventory;
using NexoPOS.Domain.MobileKits;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Cálculos de existencias compartidos entre los servicios de aplicación, para
/// que el total consolidado (sucursales + botiquines) se calcule siempre igual.
/// </summary>
internal static class InventoryMath
{
    public static decimal BranchStock(string productId, IReadOnlyList<InventoryStock> stock) =>
        stock.Where(s => s.ProductId == productId).Sum(s => s.Quantity);

    public static decimal MobileKitStock(string productId, IReadOnlyList<MobileKitStock> kitStock) =>
        kitStock.Where(s => s.ProductId == productId).Sum(s => s.Quantity);

    public static decimal TotalStock(
        string productId,
        IReadOnlyList<InventoryStock> stock,
        IReadOnlyList<MobileKitStock> kitStock) =>
        BranchStock(productId, stock) + MobileKitStock(productId, kitStock);
}
