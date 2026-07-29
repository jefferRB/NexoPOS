using NexoPOS.Domain.Catalog;

namespace NexoPOS.Infrastructure.Demo.Seed;

internal static class SupplierSeed
{
    public const string VeterinaryDistributorId = "sup-vet";
    public const string AnimalNutritionId = "sup-nutri";

    public static readonly IReadOnlyList<Supplier> Suppliers =
    [
        new Supplier { Id = VeterinaryDistributorId, Name = "Distribuidora Veterinaria CR", ContactPhone = "2233-4000" },
        new Supplier { Id = AnimalNutritionId, Name = "Nutrición Animal S.A.", ContactPhone = "2233-5000" }
    ];
}
