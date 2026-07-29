namespace NexoPOS.Domain.Branches;

/// <summary>Un día del desempeño reciente de una sucursal (dato ilustrativo de demostración).</summary>
public sealed class BranchDailyPerformance
{
    public required string BranchId { get; init; }
    public DateOnly Date { get; init; }
    public int Tickets { get; init; }
    public decimal Sales { get; init; }
}
