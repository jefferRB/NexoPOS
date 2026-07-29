using NexoPOS.Domain.Catalog;
using NexoPOS.Domain.Services;
using static NexoPOS.Infrastructure.Demo.Seed.ProductSeed;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>
/// Servicios compuestos. Los componentes con <c>ProductId</c> descuentan
/// inventario real al facturarse; los que no lo tienen son insumos genéricos o
/// tiempo de veterinario.
/// </summary>
internal static class ServiceSeed
{
    public static readonly IReadOnlyList<ServiceDefinition> Services =
    [
        new ServiceDefinition
        {
            Id = "svc-01", Name = "Consulta general",
            Description = "Valoración veterinaria general para diagnóstico y seguimiento.",
            DurationMinutes = 30, Price = 15_000m,
            Components =
            [
                new ServiceComponent { Label = "Tiempo de veterinario", DurationMinutes = 30 }
            ]
        },
        new ServiceDefinition
        {
            Id = "svc-02", Name = "Limpieza dental básica",
            Description = "Limpieza y remoción de sarro bajo sedación ligera.",
            DurationMinutes = 60, Price = 65_000m,
            Components =
            [
                new ServiceComponent { ProductId = AnesthesiaId, Label = "Anestesia veterinaria", Quantity = 10m, Unit = MeasurementUnit.Milliliter },
                new ServiceComponent { ProductId = GlovesId, Label = "Par de guantes de nitrilo", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { ProductId = SyringeId, Label = "Jeringa desechable 5 ml", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { Label = "Material clínico complementario" },
                new ServiceComponent { Label = "Tiempo de veterinario", DurationMinutes = 60 }
            ]
        },
        new ServiceDefinition
        {
            Id = "svc-03", Name = "Curación a domicilio",
            Description = "Limpieza y curación de heridas en la residencia del paciente.",
            DurationMinutes = 45, Price = 42_000m,
            Components =
            [
                new ServiceComponent { ProductId = SalineId, Label = "Suero fisiológico", Quantity = 20m, Unit = MeasurementUnit.Milliliter },
                new ServiceComponent { ProductId = GlovesId, Label = "Par de guantes de nitrilo", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { ProductId = SyringeId, Label = "Jeringa desechable 5 ml", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { Label = "Material de curación complementario" },
                new ServiceComponent { Label = "Tiempo de veterinario", DurationMinutes = 45 }
            ]
        },
        new ServiceDefinition
        {
            Id = "svc-04", Name = "Vacunación canina",
            Description = "Aplicación de vacuna múltiple canina con valoración breve.",
            DurationMinutes = 15, Price = 18_500m,
            Components =
            [
                new ServiceComponent { ProductId = CanineVaccineId, Label = "Vacuna múltiple canina", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { ProductId = SyringeId, Label = "Jeringa desechable 5 ml", Quantity = 1m, Unit = MeasurementUnit.Unit },
                new ServiceComponent { Label = "Tiempo de veterinario", DurationMinutes = 15 }
            ]
        }
    ];
}
