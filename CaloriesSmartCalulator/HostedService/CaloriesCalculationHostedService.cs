using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.HostedService
{
    public class CaloriesCalculationHostedService : BackgroundService
    {
        private readonly ILogger<CaloriesCalculationHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IMediator _mediator;
        private IRepository<CaloriesCalculationTaskItem> _repository;

        public CaloriesCalculationHostedService(ILogger<CaloriesCalculationHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        _repository = scope.ServiceProvider.GetRequiredService<IRepository<CaloriesCalculationTaskItem>>();
                        _logger.LogInformation($"starter background worker");
                        await StarCalculationTasks();
                        await Task.Delay(5000);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await Task.Delay(1000);
                }
            }
        }

        private async Task StarCalculationTasks()
        {
            var caloriesCalculationTasks = await _repository.FilterAsync(x => x.FinishedOn == null && x.FailedOn == null, 10);

            _logger.LogInformation($"total amount of work {caloriesCalculationTasks.Count()}");

            foreach (var task in caloriesCalculationTasks)
                await _mediator.Send(new CalulteCaloriesForAProductCommand(task.Id));
        }
    }
}
