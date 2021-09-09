using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Queries;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.Handlers.QueryHandlers
{
    public class GetCaloriesCalculationTaskQueryHandler : IRequestHandler<GetCaloriesCalculationTaskStatusQuery, CaloriesCalculationTask>,
                                                          IRequestHandler<GetCaloriesCalculationTaskResultQuery, CaloriesCalculationTask>
    {
        private readonly IRepository<CaloriesCalculationTask> _caloriesCalculationTaskRespository;

        public GetCaloriesCalculationTaskQueryHandler(IRepository<CaloriesCalculationTask> caloriesCalculationTaskRespository)
        {
            _caloriesCalculationTaskRespository = caloriesCalculationTaskRespository;
        }

        public async Task<CaloriesCalculationTask> Handle(GetCaloriesCalculationTaskStatusQuery request, CancellationToken cancellationToken)
        {
            return await GetCaloriesCalculationTask(request.Id);
        }

        public async Task<CaloriesCalculationTask> Handle(GetCaloriesCalculationTaskResultQuery request, CancellationToken cancellationToken)
        {
            return await GetCaloriesCalculationTask(request.Id);
        }

        public Task<CaloriesCalculationTask> GetCaloriesCalculationTask(Guid id)
        {
            return _caloriesCalculationTaskRespository.GetAsync(id);
        }
    }

}
