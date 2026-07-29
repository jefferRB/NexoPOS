using NexoPOS.Domain.Catalog;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Catálogo de productos veterinarios. Los promedios de venta semanal y mensual
/// se usan por <c>ReorderService</c> para calcular cobertura y cantidad sugerida;
/// se fijaron a mano para que un subconjunto realista requiera reposición.
/// </summary>
internal static class ProductSeed
{
    public const string AnesthesiaId = "p01";
    public const string SalineId = "p02";
    public const string AdultFoodId = "p03";
    public const string RenalFoodId = "p04";
    public const string CanineVaccineId = "p05";
    public const string FelineVaccineId = "p06";
    public const string SyringeId = "p07";
    public const string GlovesId = "p08";
    public const string ShampooId = "p09";
    public const string DewormerId = "p10";
    public const string SuturesId = "p11";
    public const string EarCleanerId = "p12";

    public static readonly IReadOnlyList<Product> Products =
    [
        new Product
        {
            Id = AnesthesiaId, Sku = "MED-001", ManufacturerBarcode = "7750000000011",
            Name = "Anestesia veterinaria 100 ml", Category = "Medicamentos",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Milliliter,
            MinimumStock = 300m, UnitPrice = 180m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = new ProductPresentation { PurchaseUnitLabel = "Frasco 100 ml", BaseUnitsPerPurchaseUnit = 100m },
            WeeklyAverageSales = 1075m, MonthlyAverageSales = 4655m
        },
        new Product
        {
            Id = SalineId, Sku = "MED-002", ManufacturerBarcode = "7750000000028",
            Name = "Suero fisiológico 500 ml", Category = "Medicamentos",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Milliliter,
            MinimumStock = 600m, UnitPrice = 25m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = new ProductPresentation { PurchaseUnitLabel = "Bolsa 500 ml", BaseUnitsPerPurchaseUnit = 500m },
            WeeklyAverageSales = 320m, MonthlyAverageSales = 1386m
        },
        new Product
        {
            Id = AdultFoodId, Sku = "ALI-001", ManufacturerBarcode = "7750000000035",
            Name = "Alimento premium adulto 20 kg", Category = "Alimentos",
            Type = ProductType.Food, BaseUnit = MeasurementUnit.Kilogram,
            MinimumStock = 40m, UnitPrice = 3200m, SupplierId = SupplierSeed.AnimalNutritionId,
            Presentation = new ProductPresentation { PurchaseUnitLabel = "Saco 20 kg", BaseUnitsPerPurchaseUnit = 20m },
            WeeklyAverageSales = 252m, MonthlyAverageSales = 1091m
        },
        new Product
        {
            Id = RenalFoodId, Sku = "ALI-002", ManufacturerBarcode = "7750000000042",
            Name = "Alimento renal 10 kg", Category = "Alimentos",
            Type = ProductType.Food, BaseUnit = MeasurementUnit.Kilogram,
            MinimumStock = 20m, UnitPrice = 4100m, SupplierId = SupplierSeed.AnimalNutritionId,
            Presentation = new ProductPresentation { PurchaseUnitLabel = "Saco 10 kg", BaseUnitsPerPurchaseUnit = 10m },
            WeeklyAverageSales = 75m, MonthlyAverageSales = 325m
        },
        new Product
        {
            Id = CanineVaccineId, Sku = "MED-003", ManufacturerBarcode = "7750000000059",
            Name = "Vacuna múltiple canina", Category = "Medicamentos",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 15m, UnitPrice = 12_500m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 128m, MonthlyAverageSales = 554m
        },
        new Product
        {
            Id = FelineVaccineId, Sku = "MED-004", ManufacturerBarcode = "7750000000066",
            Name = "Vacuna triple felina", Category = "Medicamentos",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 12m, UnitPrice = 13_800m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 28m, MonthlyAverageSales = 121m
        },
        new Product
        {
            Id = SyringeId, Sku = "INS-001", ManufacturerBarcode = "7750000000073",
            Name = "Jeringa desechable 5 ml", Category = "Insumos clínicos",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 100m, UnitPrice = 150m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 260m, MonthlyAverageSales = 1126m
        },
        new Product
        {
            Id = GlovesId, Sku = "INS-002", ManufacturerBarcode = "7750000000080",
            Name = "Guantes de nitrilo", Category = "Insumos clínicos",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 150m, UnitPrice = 120m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 300m, MonthlyAverageSales = 1299m
        },
        new Product
        {
            Id = ShampooId, Sku = "HIG-001", ManufacturerBarcode = "7750000000097",
            Name = "Shampoo medicado 250 ml", Category = "Higiene",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 10m, UnitPrice = 6500m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 22m, MonthlyAverageSales = 95m
        },
        new Product
        {
            Id = DewormerId, Sku = "MED-005", ManufacturerBarcode = "7750000000103",
            Name = "Antiparasitario oral", Category = "Medicamentos",
            Type = ProductType.Medication, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 20m, UnitPrice = 8900m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 22m, MonthlyAverageSales = 95m
        },
        new Product
        {
            Id = SuturesId, Sku = "INS-003", ManufacturerBarcode = "7750000000110",
            Name = "Suturas absorbibles", Category = "Insumos clínicos",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 25m, UnitPrice = 3400m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 20m, MonthlyAverageSales = 87m
        },
        new Product
        {
            Id = EarCleanerId, Sku = "HIG-002", ManufacturerBarcode = "7750000000127",
            Name = "Limpiador ótico 120 ml", Category = "Higiene",
            Type = ProductType.ClinicalSupply, BaseUnit = MeasurementUnit.Unit,
            MinimumStock = 12m, UnitPrice = 4200m, SupplierId = SupplierSeed.VeterinaryDistributorId,
            Presentation = null, WeeklyAverageSales = 9m, MonthlyAverageSales = 39m
        }
    ];
}
