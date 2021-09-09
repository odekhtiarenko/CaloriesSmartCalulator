using CaloriesSmartCalulator.Handlers.Contracts.Results;
using MediatR;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Commands
{
    public class CalulteCaloriesForAProductCommand : IRequest<CalulteCaloriesForAProductResult>
    {
        public CalulteCaloriesForAProductCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
