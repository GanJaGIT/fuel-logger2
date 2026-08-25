using System.Collections.Generic;

namespace FuelLogger.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public int Level { get; set; } // 0 - верхнее, 1 - нижнее

        public Department? Parent { get; set; }
        public List<Department> Children { get; set; } = new List<Department>();
        public List<FleetItem> FleetItems { get; set; } = new List<FleetItem>();
        public List<FuelTask> Tasks { get; set; } = new List<FuelTask>();

        public bool IsChecked { get; set; }
    }
}