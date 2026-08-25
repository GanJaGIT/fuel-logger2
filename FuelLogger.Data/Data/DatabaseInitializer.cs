using System.Linq;
using FuelLogger.Core.Models;

namespace FuelLogger.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "РВО" },
                    new Category { Name = "АС" },
                    new Category { Name = "РХБЗ" },
                    new Category { Name = "ПС" },
                    new Category { Name = "ВС" },
                    new Category { Name = "ГСМ" },
                    new Category { Name = "Связь" }
                );
                context.SaveChanges();
            }

            if (!context.TaskTypes.Any())
            {
                context.TaskTypes.AddRange(
                    new TaskType { Name = "Подвоз воды" },
                    new TaskType { Name = "Боевое дежурство" },
                    new TaskType { Name = "Разведка" }
                );
                context.SaveChanges();
            }
        }
    }
}