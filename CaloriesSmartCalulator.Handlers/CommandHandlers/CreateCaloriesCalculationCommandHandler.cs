using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.Handlers.CommandHandlers
{
    public class CreateCaloriesCalculationCommandHandler : IRequestHandler<CreateCaloriesCalculationCommand, CreateCaloriesCalculationResult>
    {
        private readonly IRepository<CaloriesCalculationTask> _caloriesCalculationTaskRespository;

        public CreateCaloriesCalculationCommandHandler(IRepository<CaloriesCalculationTask> caloriesCalculationTaskRespository)
        {
            _caloriesCalculationTaskRespository = caloriesCalculationTaskRespository;
        }

        public async Task<CreateCaloriesCalculationResult> Handle(CreateCaloriesCalculationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = new CaloriesCalculationTask();
                task.CaloriesCalculationTaskItems = request.Products.Select(v => new CaloriesCalculationTaskItem() { Product = v }).ToList();
                var result = await _caloriesCalculationTaskRespository.InsertAsync(task);

                return new CreateCaloriesCalculationResult(result.Id);
            }
            catch (Exception ex)
            {
                return new CreateCaloriesCalculationResult(ex);
            }
        }
    }
}
