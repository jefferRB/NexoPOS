namespace NexoPOS.Domain.Services;

/// <summary>
/// Un servicio o paquete que la veterinaria ofrece, compuesto por productos e
/// insumos de inventario y tiempo de veterinario.
/// </summary>
public sealed class ServiceDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public int DurationMinutes { get; init; }
    public decimal Price { get; init; }
    public required IReadOnlyList<ServiceComponent> Components { get; init; }
}
