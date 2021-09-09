using System;

namespace CaloriesSmartCalulator.Data.Entities
{
    public class CaloriesCalculationTaskItem: IEntity
    {
        public Guid Id { get; set; }
        public Guid CaloriesCalculationTaskId { get; set; }
        public string Product { get; set; }
        public int Calories { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedOn { get; set; }
        public DateTime? FailedOn { get; set; }
        public CaloriesCalculationTask CaloriesCalculationTask { get; set; }
    }
}
