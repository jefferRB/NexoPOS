namespace NexoPOS.Domain.Inventory;

/// <summary>Estado de las existencias de un producto.</summary>
public enum StockStatus
{
    /// <summary>Existencias por encima del mínimo.</summary>
    Available,

    /// <summary>Existencias iguales o por debajo del mínimo, pero disponibles.</summary>
    Low,

    /// <summary>Sin existencias.</summary>
    OutOfStock
}

/// <summary>
/// Regla de negocio que determina el estado de las existencias. Vive en el
/// dominio para evitar duplicar el criterio en otras capas. Trabaja en decimal
/// porque los productos fraccionables administran fracciones de la unidad base.
/// </summary>
public static class StockStatusRules
{
    public static StockStatus Evaluate(decimal quantity, decimal minimumStock)
    {
        if (quantity <= 0)
        {
            return StockStatus.OutOfStock;
        }

        return quantity <= minimumStock ? StockStatus.Low : StockStatus.Available;
    }

    /// <summary>Indica si el estado corresponde a una alerta de inventario.</summary>
    public static bool IsAlert(StockStatus status) => status != StockStatus.Available;
}
