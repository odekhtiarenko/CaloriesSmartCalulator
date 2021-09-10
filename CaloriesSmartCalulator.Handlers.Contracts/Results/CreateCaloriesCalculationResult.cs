using CaloriesSmartCalulator.Data.Entities;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Results
{
    public class CreateCaloriesCalculationResult : OperationResultBase<CaloriesCalculationTask>
    {
        public CreateCaloriesCalculationResult(CaloriesCalculationTask value) : base(value)
        {
        }

        public CreateCaloriesCalculationResult(string message) : base(message)
        {
        }

        public CreateCaloriesCalculationResult(Exception ex) : base(ex)
        {
        }
    }
}
