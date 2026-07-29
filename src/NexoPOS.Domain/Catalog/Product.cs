namespace NexoPOS.Domain.Catalog;

/// <summary>
/// Un producto del catálogo. El stock mínimo y las existencias se expresan en
/// <see cref="BaseUnit"/>. Cuando <see cref="Presentation"/> no es nulo, el
/// producto es fraccionable: se compra en una presentación pero se administra y
/// se vende por fracción de la unidad base (por ejemplo, mililitros o kilogramos).
/// </summary>
public sealed class Product
{
    public required string Id { get; init; }

    /// <summary>Código interno de la empresa.</summary>
    public required string Sku { get; init; }

    /// <summary>Código de barras del fabricante.</summary>
    public required string ManufacturerBarcode { get; init; }

    public required string Name { get; init; }
    public required string Category { get; init; }
    public ProductType Type { get; init; }
    public MeasurementUnit BaseUnit { get; init; }

    /// <summary>Existencia mínima antes de considerar el stock como bajo, en <see cref="BaseUnit"/>.</summary>
    public decimal MinimumStock { get; init; }

    /// <summary>Precio de venta por unidad base, en colones (CRC).</summary>
    public decimal UnitPrice { get; init; }

    public required string SupplierId { get; init; }

    /// <summary>Presente solo si el producto es fraccionable.</summary>
    public ProductPresentation? Presentation { get; init; }

    public bool IsFractionable => Presentation is not null;

    /// <summary>Venta promedio semanal en <see cref="BaseUnit"/>, usada para calcular reposición.</summary>
    public decimal WeeklyAverageSales { get; init; }

    /// <summary>Venta promedio mensual en <see cref="BaseUnit"/>, usada para calcular reposición.</summary>
    public decimal MonthlyAverageSales { get; init; }
}
