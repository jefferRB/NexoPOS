namespace NexoPOS.Domain.Branches;

/// <summary>
/// Una sucursal física de la empresa. Los importes están expresados en colones
/// costarricenses (CRC).
/// </summary>
public sealed class Branch
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Location { get; init; }
    public required string Phone { get; init; }
    public required string Schedule { get; init; }

    /// <summary>Indica si la sucursal está operativa en este momento.</summary>
    public bool IsOperational { get; init; }

    /// <summary>Veterinarios y colaboradores activos actualmente en la sucursal.</summary>
    public int ActiveCollaborators { get; init; }

    /// <summary>Ventas del día en colones (CRC).</summary>
    public decimal SalesToday { get; init; }

    /// <summary>Tiquetes o facturas emitidas hoy.</summary>
    public int TicketsToday { get; init; }

    /// <summary>Saldo de cuentas por cobrar asociado a la sucursal, en CRC.</summary>
    public decimal ReceivablesBalance { get; init; }

    /// <summary>Saldo de cuentas por pagar asociado a la sucursal, en CRC.</summary>
    public decimal PayablesBalance { get; init; }
}
