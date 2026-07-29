namespace NexoPOS.Application.Demo.Dtos;

/// <summary>Un componente de un servicio compuesto, para el modal "Ver composición".</summary>
public sealed record ServiceComponentDto(
    string? ProductId,
    string Label,
    decimal? Quantity,
    string? Unit,
    int? DurationMinutes,
    bool LinksToInventory);

/// <summary>Un servicio o paquete ofrecido por la veterinaria.</summary>
public sealed record ServiceDto(
    string Id,
    string Name,
    string Description,
    int DurationMinutes,
    decimal Price,
    IReadOnlyList<ServiceComponentDto> Components);
