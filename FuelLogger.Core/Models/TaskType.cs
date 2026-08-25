using System.Collections.Generic;

namespace FuelLogger.Core.Models
{
    public class TaskType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<FuelTask> Tasks { get; set; } = new List<FuelTask>();
    }
}