using System;

namespace FuelLogger.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public double DensityGasoline { get; set; } = 0.75;
        public double DensityDiesel { get; set; } = 0.85;
        public string Theme { get; set; } = "Light";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string FolderPath { get; set; } = string.Empty;
        public string DatabasePath => System.IO.Path.Combine(FolderPath, "fuel.db");
    }
}