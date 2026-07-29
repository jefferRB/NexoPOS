namespace NexoPOS.Domain.Branches;

/// <summary>Producto más vendido de una sucursal en la semana (dato ilustrativo de demostración).</summary>
public sealed class BranchTopProduct
{
    public required string BranchId { get; init; }
    public required string ProductId { get; init; }
    public decimal QuantitySold { get; init; }
}
