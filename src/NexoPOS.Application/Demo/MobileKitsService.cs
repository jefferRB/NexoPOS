using NexoPOS.Application.Demo.Abstractions;
using NexoPOS.Application.Demo.Dtos;
using NexoPOS.Domain.Activity;

namespace NexoPOS.Application.Demo;

/// <summary>
/// Arma la vista de botiquines móviles: contenido, valor estimado y actividad
/// propia de cada botiquín (transferencias, consumos, devoluciones).
/// </summary>
public sealed class MobileKitsService(IDemoOperationsRepository repository) : IMobileKitsService
{
    public async Task<IReadOnlyList<MobileKitDto>> GetMobileKitsAsync(CancellationToken cancellationToken = default)
    {
        var kits = await repository.GetMobileKitsAsync(cancellationToken);
        var kitStock = await repository.GetMobileKitStockAsync(cancellationToken);
        var products = await repository.GetProductsAsync(cancellationToken);
        var branches = await repository.GetBranchesAsync(cancellationToken);
        var activity = await repository.GetRecentActivityAsync(cancellationToken);

        var productById = products.ToDictionary(p => p.Id);
        var branchNames = branches.ToDictionary(b => b.Id, b => b.Name);

        return kits
            .Select(kit =>
            {
                var stockLines = kitStock
                    .Where(s => s.MobileKitId == kit.Id && productById.ContainsKey(s.ProductId))
                    .Select(s =>
                    {
                        var product = productById[s.ProductId];
                        return new MobileKitStockLineDto(
                            ProductId: product.Id,
                            ProductName: product.Name,
                            Quantity: s.Quantity,
                            Unit: DemoPresentation.UnitCode(product.BaseUnit),
                            EstimatedValue: decimal.Round(s.Quantity * product.UnitPrice, 2));
                    })
                    .OrderBy(l => l.ProductName, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var kitActivity = activity
                    .Where(e => e.MobileKitId == kit.Id)
                    .OrderByDescending(e => e.Timestamp)
                    .Select(e => MapActivity(e, branchNames, kit.Name))
                    .ToList();

                return new MobileKitDto(
                    Id: kit.Id,
                    Name: kit.Name,
                    AssignedTo: kit.AssignedTo,
                    HomeBranchId: kit.HomeBranchId,
                    HomeBranchName: branchNames.GetValueOrDefault(kit.HomeBranchId, kit.HomeBranchId),
                    Status: DemoPresentation.MobileKitStatusCode(kit.Status),
                    LastTransferAt: kit.LastTransferAt,
                    LastConsumptionAt: kit.LastConsumptionAt,
                    EstimatedValue: stockLines.Sum(l => l.EstimatedValue),
                    Alerts: kit.Alerts,
                    Stock: stockLines,
                    RecentActivity: kitActivity);
            })
            .ToList();
    }

    private static ActivityDto MapActivity(ActivityEvent e, IReadOnlyDictionary<string, string> branchNames, string kitName) =>
        new(
            Id: e.Id,
            Type: DemoPresentation.ActivityCode(e.Type),
            Action: DemoPresentation.ActivityLabel(e.Type),
            UserName: e.UserName,
            UserRole: e.UserRole,
            LocationName: kitName,
            BranchId: e.BranchId,
            MobileKitId: e.MobileKitId,
            Timestamp: e.Timestamp,
            Reference: e.Reference,
            Reason: e.Reason,
            Amount: e.Amount);
}
