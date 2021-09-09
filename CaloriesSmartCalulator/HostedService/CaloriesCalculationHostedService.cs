using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.HostedService
{
    public class CaloriesCalculationHostedService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
