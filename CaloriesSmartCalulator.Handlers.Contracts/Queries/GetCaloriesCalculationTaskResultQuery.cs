using CaloriesSmartCalulator.Data.Entities;
using MediatR;
using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Queries
{
    public class GetCaloriesCalculationTaskResultQuery : IRequest<CaloriesCalculationTask>
    {
        public Guid Id { get; set; }

        public GetCaloriesCalculationTaskResultQuery(Guid id)
        {
            Id = id;
        }
    }
}
