namespace FuelLogger.Core.Models
{
    public class TemplateVehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FuelType { get; set; } // "AB" или "DT"
        public double TankVolume { get; set; }
        public double CarriedFuel { get; set; }
        public double Consumption100km { get; set; }
        public double ConsumptionPerHour { get; set; }
        public string Category { get; set; }
        
        // Навигационные свойства
        public List<FleetItem> FleetItems { get; set; } = new List<FleetItem>();
    }
}