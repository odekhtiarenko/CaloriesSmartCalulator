using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.Handlers.CommandHandlers
{
    public class FinishCalculationTaskCommandHandler : IRequestHandler<FinishCalculationTaskCommand>
    {
        private readonly IRepository<CaloriesCalculationTask> _caloriesCalculationTaskRespository;
        private readonly IMediator _mediator;
        private readonly ILogger<FinishCalculationTaskCommandHandler> _logger;

        public FinishCalculationTaskCommandHandler(IRepository<CaloriesCalculationTask> caloriesCalculationTaskRespository,
                                                   IMediator mediator,
                                                   ILogger<FinishCalculationTaskCommandHandler> logger)
        {
            _caloriesCalculationTaskRespository = caloriesCalculationTaskRespository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(FinishCalculationTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"finishing task {request.Id} Thread: {Thread.CurrentThread.ManagedThreadId}");
            var task = await _caloriesCalculationTaskRespository.GetAsync(request.Id);

            if (task.CaloriesCalculationTaskItems.All(x => x.FinishedOn != null))
            {
                task.FinishedOn = DateTime.UtcNow;
                return await FinishTask(task);
            }

            if (task.CaloriesCalculationTaskItems.Any(x => x.FailedOn != null))
            {
                task.FailedOn = DateTime.UtcNow;
                return await FinishTask(task);
            }

            _logger.LogWarning($"Task {request.Id} has unfinished items");
            await _mediator.Send(request);
            await Task.Delay(500);
            return Unit.Value;
        }

        private Task<Unit> FinishTask(CaloriesCalculationTask task)
        {
            _caloriesCalculationTaskRespository.UpdateAsync(task);
            return Task.FromResult(Unit.Value);

        }
    }
}
