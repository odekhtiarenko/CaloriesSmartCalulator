using MediatR;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Commands
{
    public class FinishCalculationTaskCommand : IRequest
    {
        public FinishCalculationTaskCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
