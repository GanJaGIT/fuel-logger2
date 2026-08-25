using System.Collections.Generic;

namespace FuelLogger.Core.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; } // 0 - верхнее, 1 - нижнее
        
        // Навигационные свойства
        public Department Parent { get; set; }
        public List<Department> Children { get; set; } = new List<Department>();
        public List<FleetItem> FleetItems { get; set; } = new List<FleetItem>();
        public List<Task> Tasks { get; set; } = new List<Task>();
        
        // Для UI
        public bool IsChecked { get; set; }
    }
}