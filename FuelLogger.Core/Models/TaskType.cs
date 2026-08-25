namespace FuelLogger.Core.Models
{
    public class TaskType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}