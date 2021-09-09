using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Results
{
    public class CalulteCaloriesForAProductResult : OperationResultBase<Guid>
    {
        public CalulteCaloriesForAProductResult(Guid value) : base(value)
        {
        }

        public CalulteCaloriesForAProductResult(string message) : base(message)
        {
        }

        public CalulteCaloriesForAProductResult(Exception ex) : base(ex)
        {
        }
    }
}
