using System.Collections.Generic;

namespace FuelLogger.Core.Models
{
    public class TemplateVehicle
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty; // "AB" или "DT"
        public double TankVolume { get; set; }
        public double CarriedFuel { get; set; }
        public double Consumption100km { get; set; }
        public double ConsumptionPerHour { get; set; }
        public string Category { get; set; } = string.Empty;

        public List<FleetItem> FleetItems { get; set; } = new List<FleetItem>();
    }
}