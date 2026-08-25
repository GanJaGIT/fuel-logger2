using System;
using System.Collections.Generic;

namespace FuelLogger.Core.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int TaskTypeId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Навигационные свойства
        public TaskType TaskType { get; set; }
        public Department Department { get; set; }
        public List<TaskVehicle> Vehicles { get; set; } = new List<TaskVehicle>();
        
        // Для UI
        public bool IsSelected { get; set; }
    }
}