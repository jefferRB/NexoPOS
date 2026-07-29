using NexoPOS.Application.Demo.Dtos;

namespace NexoPOS.Application.Demo.Abstractions;

/// <summary>Base de cálculo del consumo promedio usada para sugerir reposición.</summary>
public enum ReorderBasis
{
    Weekly,
    Monthly
}

/// <summary>
/// Calcula sugerencias de reposición a partir del consumo promedio y las
/// existencias actuales (sucursales + botiquines), agrupadas por proveedor.
/// </summary>
public interface IReorderService
{
    Task<ReorderResponseDto> GetReorderSuggestionsAsync(ReorderBasis basis, CancellationToken cancellationToken = default);

    /// <summary>Cantidad de productos que requieren reposición según la base indicada (para indicadores del panel).</summary>
    Task<int> GetSuggestedCountAsync(ReorderBasis basis, CancellationToken cancellationToken = default);
}
