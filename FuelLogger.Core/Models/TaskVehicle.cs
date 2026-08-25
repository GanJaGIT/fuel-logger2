namespace FuelLogger.Core.Models
{
    public class TaskVehicle
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int FleetId { get; set; }
        public int UsedQuantity { get; set; }
        public double? DistanceKm { get; set; }
        public double? TimeHours { get; set; }
        public double? FuelConsumptionLiters { get; set; }
        public double? FuelConsumptionKg { get; set; }
        public double? FuelConsumptionTanks { get; set; }

        public FuelTask Task { get; set; }
        public FleetItem FleetItem { get; set; }
    }
}