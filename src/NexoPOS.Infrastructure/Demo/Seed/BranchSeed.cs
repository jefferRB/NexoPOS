using NexoPOS.Domain.Branches;

namespace NexoPOS.Infrastructure.Demo.Seed;

/// <summary>Las tres veterinarias de Grupo Veterinario Demo.</summary>
internal static class BranchSeed
{
    public const string SanJoseId = "sanjose";
    public const string HerediaId = "heredia";
    public const string CartagoId = "cartago";

    public static readonly IReadOnlyList<Branch> Branches =
    [
        new Branch
        {
            Id = SanJoseId,
            Name = "Veterinaria San José",
            Location = "San José, Barrio Escalante",
            Phone = "2222-1001",
            Schedule = "Lunes a sábado, 8:00 a. m. – 6:00 p. m.",
            IsOperational = true,
            ActiveCollaborators = 6,
            SalesToday = 1_420_350m,
            TicketsToday = 68,
            ReceivablesBalance = 450_000m,
            PayablesBalance = 610_000m
        },
        new Branch
        {
            Id = HerediaId,
            Name = "Veterinaria Heredia",
            Location = "Heredia, San Francisco",
            Phone = "2222-1002",
            Schedule = "Lunes a sábado, 8:00 a. m. – 6:00 p. m.",
            IsOperational = true,
            ActiveCollaborators = 4,
            SalesToday = 986_800m,
            TicketsToday = 57,
            ReceivablesBalance = 280_000m,
            PayablesBalance = 390_000m
        },
        new Branch
        {
            Id = CartagoId,
            Name = "Veterinaria Cartago",
            Location = "Cartago, El Molino",
            Phone = "2222-1003",
            Schedule = "Lunes a sábado, 8:00 a. m. – 6:00 p. m.",
            IsOperational = true,
            ActiveCollaborators = 3,
            SalesToday = 821_450m,
            TicketsToday = 53,
            ReceivablesBalance = 195_000m,
            PayablesBalance = 275_000m
        }
    ];
}
