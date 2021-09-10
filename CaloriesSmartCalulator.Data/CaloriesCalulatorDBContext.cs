using CaloriesSmartCalulator.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaloriesSmartCalulator.Data
{
    public class CaloriesCalulatorDBContext : DbContext
    {
        public DbSet<CaloriesCalculationTask> CaloriesCalculationTasks { get; set; }
        public DbSet<CaloriesCalculationTaskItem> CaloriesCalculationTaskItems { get; set; }

        public CaloriesCalulatorDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CaloriesCalculationTask>()
                .HasKey(key => key.Id);

            modelBuilder
                .Entity<CaloriesCalculationTask>()
                .Property(t=>t.Name)
                .HasMaxLength(100);

            modelBuilder
                .Entity<CaloriesCalculationTask>()
                .HasMany(tI => tI.CaloriesCalculationTaskItems)
                .WithOne(t => t.CaloriesCalculationTask);

            modelBuilder
                .Entity<CaloriesCalculationTaskItem>()
                .HasKey(key => key.Id);
        }
    }
}
