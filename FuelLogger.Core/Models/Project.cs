using System;

namespace FuelLogger.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double DensityGasoline { get; set; } = 0.75;
        public double DensityDiesel { get; set; } = 0.85;
        public string Theme { get; set; } = "Light";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Для UI
        public string FolderPath { get; set; }
        public string DatabasePath => System.IO.Path.Combine(FolderPath, "fuel.db");
    }
}