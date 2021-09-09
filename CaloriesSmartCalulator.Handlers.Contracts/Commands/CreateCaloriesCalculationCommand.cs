using CaloriesSmartCalulator.Handlers.Contracts.Results;
using MediatR;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Commands
{
    public class CreateCaloriesCalculationCommand : IRequest<CreateCaloriesCalculationResult>
    {
        public string[] Products { get; }
      
        public CreateCaloriesCalculationCommand(string[] products)
        {
            Products = products;
        }
    }
}
