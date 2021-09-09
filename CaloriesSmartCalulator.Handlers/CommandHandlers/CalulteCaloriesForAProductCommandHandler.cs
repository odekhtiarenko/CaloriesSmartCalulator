using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Results;
using CaloriesSmartCalulator.ServiceClients;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.Handlers.CommandHandlers
{
    public class CalulteCaloriesForAProductCommandHandler : IRequestHandler<CalulteCaloriesForAProductCommand, CalulteCaloriesForAProductResult>
    {
        private readonly IRepository<CaloriesCalculationTaskItem> _caloriesCalculationTaskItemsRespository;
        private readonly ILogger<CalulteCaloriesForAProductCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICaloriesServiceClient _caloriesServiceClient;

        public CalulteCaloriesForAProductCommandHandler(IRepository<CaloriesCalculationTaskItem> caloriesCalculationTaskRespository,
                                                        ILogger<CalulteCaloriesForAProductCommandHandler> logger,
                                                        IMediator mediator,
                                                        ICaloriesServiceClient caloriesServiceClient)
        {
            _caloriesCalculationTaskItemsRespository = caloriesCalculationTaskRespository;
            _logger = logger;
            _mediator = mediator;
            _caloriesServiceClient = caloriesServiceClient;
        }

        public async Task<CalulteCaloriesForAProductResult> Handle(CalulteCaloriesForAProductCommand request, CancellationToken cancellationToken)
        {
            var taskItem = await _caloriesCalculationTaskItemsRespository.GetAsync(request.Id);
            if (taskItem == null)
                return new CalulteCaloriesForAProductResult("Taks item doesnt exists");

            try
            {
                _logger.LogInformation($"Working on {request.Id} Thread: {Thread.CurrentThread.ManagedThreadId}");
                taskItem.Calories = await _caloriesServiceClient.GetCaloriesForProduct(taskItem.Product);
                taskItem.FinishedOn = DateTime.UtcNow;
                return await UpdateTask(taskItem);
            }
            catch (Exception ex)
            {
                taskItem.FailedOn = DateTime.UtcNow;
                return await UpdateTask(taskItem, ex);
            }
        }

        private async Task<CalulteCaloriesForAProductResult> UpdateTask(CaloriesCalculationTaskItem taskItem, Exception ex = null)
        {
            var item = await _caloriesCalculationTaskItemsRespository.UpdateAsync(taskItem);
            if (ex!=null)
            {
                await SendFinishTaskCommand(item.CaloriesCalculationTask.Id);
                return new CalulteCaloriesForAProductResult(ex);
            }

            if (item.CaloriesCalculationTask.CaloriesCalculationTaskItems.All(x => x.FailedOn != null || x.FinishedOn != null))
                await SendFinishTaskCommand(item.CaloriesCalculationTask.Id);


            _logger.LogInformation($"Finished on {taskItem.Id} Thread: {Thread.CurrentThread.ManagedThreadId}");

            return new CalulteCaloriesForAProductResult(taskItem.Id);
        }

        private Task SendFinishTaskCommand(Guid id)
        {
            return _mediator.Send(new FinishCalculationTaskCommand(id));
        }
    }
}
