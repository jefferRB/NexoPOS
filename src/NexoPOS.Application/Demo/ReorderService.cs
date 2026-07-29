using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;
using NexoPOS.Domain.Catalog;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Calcula sugerencias de reposición a partir del consumo promedio (semanal o
/// mensual) y las existencias actuales (sucursales + botiquines). Un producto
/// sin historial de venta se excluye porque no hay forma de estimar su
/// cobertura. La cobertura objetivo (dos semanas) es la misma para toda
/// sugerencia; solo cambia la base de cálculo del consumo.
/// </summary>
public sealed class ReorderService(IDemoOperationsRepository repository) : IReorderService
{
    private const decimal TargetCoverageDays = 14m;

    public async Task<ReorderResponseDto> GetReorderSuggestionsAsync(ReorderBasis basis, CancellationToken cancellationToken = default)
    {
        var suggestions = await BuildSuggestionsAsync(basis, cancellationToken);
        var suppliers = await repository.GetSuppliersAsync(cancellationToken);
        var supplierNames = suppliers.ToDictionary(s => s.Id, s => s.Name);

        var supplierOrders = suggestions
            .GroupBy(s => s.Suggestion.SupplierId)
            .Select(g => new SupplierOrderDto(
                SupplierId: g.Key,
                SupplierName: supplierNames.GetValueOrDefault(g.Key, g.Key),
                EstimatedValue: g.Sum(i => i.EstimatedValue),
                Items: g.Select(i => i.Suggestion)
                    .OrderByDescending(s => PriorityRank(s.Priority))
                    .ThenBy(s => s.CoverageDays)
                    .ToList()))
            .OrderByDescending(o => o.EstimatedValue)
            .ToList();

        var indicators = new ReorderIndicatorsDto(
            ProductsToReorder: suggestions.Count,
            SuppliersInvolved: supplierOrders.Count,
            EstimatedValue: suggestions.Sum(s => s.EstimatedValue),
            AverageCoverageDays: suggestions.Count == 0
                ? 0m
                : decimal.Round(suggestions.Average(s => s.Suggestion.CoverageDays), 1));

        return new ReorderResponseDto(
            Basis: basis == ReorderBasis.Weekly ? "weekly" : "monthly",
            Indicators: indicators,
            SupplierOrders: supplierOrders);
    }

    public async Task<int> GetSuggestedCountAsync(ReorderBasis basis, CancellationToken cancellationToken = default)
    {
        var suggestions = await BuildSuggestionsAsync(basis, cancellationToken);
        return suggestions.Count;
    }

    private async Task<List<(ReorderSuggestionDto Suggestion, decimal EstimatedValue)>> BuildSuggestionsAsync(
        ReorderBasis basis, CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsAsync(cancellationToken);
        var suppliers = await repository.GetSuppliersAsync(cancellationToken);
        var stock = await repository.GetInventoryAsync(cancellationToken);
        var kitStock = await repository.GetMobileKitStockAsync(cancellationToken);
        var supplierNames = suppliers.ToDictionary(s => s.Id, s => s.Name);

        var periodDays = basis == ReorderBasis.Weekly ? 7m : 30m;
        var results = new List<(ReorderSuggestionDto, decimal)>();

        foreach (var product in products)
        {
            var averageSales = basis == ReorderBasis.Weekly ? product.WeeklyAverageSales : product.MonthlyAverageSales;
            if (averageSales <= 0)
            {
                continue;
            }

            var dailyRate = averageSales / periodDays;
            var currentStock = InventoryMath.TotalStock(product.Id, stock, kitStock);
            var coverageDays = decimal.Round(currentStock / dailyRate, 1);

            if (coverageDays >= TargetCoverageDays)
            {
                continue;
            }

            var rawSuggested = Math.Max(0m, TargetCoverageDays * dailyRate - currentStock);
            var suggestedQuantity = product.Presentation is null
                ? Math.Ceiling(rawSuggested)
                : Math.Ceiling(rawSuggested / product.Presentation.BaseUnitsPerPurchaseUnit) * product.Presentation.BaseUnitsPerPurchaseUnit;

            var priority = coverageDays < 5m ? "high" : coverageDays < 10m ? "medium" : "low";

            var suggestion = new ReorderSuggestionDto(
                ProductId: product.Id,
                ProductName: product.Name,
                ProductCode: product.Sku,
                SupplierId: product.SupplierId,
                SupplierName: supplierNames.GetValueOrDefault(product.SupplierId, product.SupplierId),
                CurrentStock: currentStock,
                Unit: DemoPresentation.UnitCode(product.BaseUnit),
                WeeklyAverageSales: product.WeeklyAverageSales,
                MonthlyAverageSales: product.MonthlyAverageSales,
                CoverageDays: coverageDays,
                SuggestedQuantity: suggestedQuantity,
                Priority: priority);

            results.Add((suggestion, suggestedQuantity * product.UnitPrice));
        }

        return results;
    }

    private static int PriorityRank(string priority) => priority switch
    {
        "high" => 2,
        "medium" => 1,
        _ => 0
    };
}
