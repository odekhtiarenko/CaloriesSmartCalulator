using CaloriesSmartCalulator.Data.Entities;
using MediatR;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Queries
{
    public class GetCaloriesCalculationTaskStatusQuery : IRequest<CaloriesCalculationTask>
    {
        public Guid Id { get; set; }

        public GetCaloriesCalculationTaskStatusQuery(Guid id)
        {
            Id = id;
        }
    }
}
