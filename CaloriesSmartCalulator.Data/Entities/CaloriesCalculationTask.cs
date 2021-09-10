using System;
using System.Collections.Generic;

namespace CaloriesSmartCalulator.Data.Entities
{
    public class CaloriesCalculationTask : IEntity
    {
        public Guid Id { get; set; }
        public ICollection<CaloriesCalculationTaskItem> CaloriesCalculationTaskItems { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime InProgressOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public DateTime? FailedOn { get; set; }
    }
}
