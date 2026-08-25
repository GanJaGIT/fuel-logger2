using Microsoft.EntityFrameworkCore;
using FuelLogger.Core.Models;

namespace FuelLogger.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _dbPath;

        public AppDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        // DbSet'ы для всех сущностей
        public DbSet<Department> Departments { get; set; }
        public DbSet<TemplateVehicle> TemplateVehicles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<FleetItem> FleetItems { get; set; }
        public DbSet<Core.Models.Task> Tasks { get; set; }
        public DbSet<TaskVehicle> TaskVehicles { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FleetItem>()
                .HasOne(f => f.Department)
                .WithMany(d => d.FleetItems)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FleetItem>()
                .HasOne(f => f.Vehicle)
                .WithMany(v => v.FleetItems)
                .HasForeignKey(f => f.TemplateVehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Tasks)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.TaskType)
                .WithMany(tt => tt.Tasks)
                .HasForeignKey(t => t.TaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskVehicle>()
                .HasOne(tv => tv.Task)
                .WithMany(t => t.Vehicles)
                .HasForeignKey(tv => tv.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskVehicle>()
                .HasOne(tv => tv.FleetItem)
                .WithMany()
                .HasForeignKey(tv => tv.FleetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Уникальность названий
            modelBuilder.Entity<TemplateVehicle>()
                .HasIndex(v => v.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<TaskType>()
                .HasIndex(tt => tt.Name)
                .IsUnique();

            // Ограничения
            modelBuilder.Entity<Department>()
                .HasCheckConstraint("CK_Department_Level", "Level IN (0, 1)");

            modelBuilder.Entity<TemplateVehicle>()
                .HasCheckConstraint("CK_TemplateVehicle_FuelType", "FuelType IN ('AB', 'DT')");
        }
    }
}