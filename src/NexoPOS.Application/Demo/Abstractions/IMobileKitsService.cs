using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>Botiquines móviles asignados a veterinarios de visita a domicilio.</summary>
public interface IMobileKitsService
{
    Task<IReadOnlyList<MobileKitDto>> GetMobileKitsAsync(CancellationToken cancellationToken = default);
}
