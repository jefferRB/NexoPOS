namespace NexoPOS.Domain.Catalog;

/// <summary>
/// Categoría principal de un producto. La fraccionabilidad es un rasgo aparte
/// (ver <see cref="Product.Presentation"/>): un medicamento o alimento puede o
/// no venderse por fracción.
/// </summary>
public enum ProductType
{
    Standard,
    Medication,
    Food,
    ClinicalSupply
}
