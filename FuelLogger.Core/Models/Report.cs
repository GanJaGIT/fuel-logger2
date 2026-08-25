using System;

namespace FuelLogger.Core.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string TaskIds { get; set; } = string.Empty;
    }
}