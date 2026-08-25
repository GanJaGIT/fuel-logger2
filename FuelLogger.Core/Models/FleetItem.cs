namespace FuelLogger.Core.Models
{
    public class FleetItem
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int TemplateVehicleId { get; set; }
        public int Quantity { get; set; }

        public Department Department { get; set; }
        public TemplateVehicle Vehicle { get; set; }

        public double TotalTankVolume => Quantity * (Vehicle?.TankVolume ?? 0);
    }
}