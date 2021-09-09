using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Results
{
    public class CreateCaloriesCalculationResult : OperationResultBase<Guid>
    {
        public CreateCaloriesCalculationResult(Guid value) : base(value)
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
