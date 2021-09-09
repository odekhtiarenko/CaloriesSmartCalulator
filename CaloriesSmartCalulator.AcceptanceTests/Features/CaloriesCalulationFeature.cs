using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.AcceptanceTests.Features
{
    public partial class CaloriesCalulationFeature
    {
        [Scenario]
        [ScenarioCategory(Categories.CaloriesCalculator)]
        public async Task CreateCaloriesCalculationTask()
        {
            await Runner.AddSteps(Given_ProductsToCalculateCalories)
                        .AddAsyncSteps(_ => Then_CallToEndpointShouldCreateCalculationTask(),
                                       _ => And_TaskShouldBeCreated())
                        .RunAsync();
        }

        [Scenario]
        [ScenarioCategory(Categories.CaloriesCalculator)]
        public async Task CreateCaloriesCalculationTaskShouldCalculateSuccessfuly()
        {
            await Runner.AddSteps(Given_ProductsToCalculateCalories)
                        .AddAsyncSteps(_ => Then_CallToEndpointShouldCreateCalculationTask(),
                                       _ => And_TaskShouldBeCreated(),
                                       _ => And_TaskShouldBeCalculated(),
                                       _=>Then_TaskResultShouldBeRetrivedWithPRoperStatus(Dtos.Status.Completed))
                        .RunAsync();
        }

        [Scenario]
        [ScenarioCategory(Categories.CaloriesCalculator)]
        public async Task CreateCaloriesCalculationTaskShouldCalculateFailed()
        {
            await Runner.AddSteps(Given_FailedProductsToCalculateCalories)
                        .AddAsyncSteps(_ => Then_CallToEndpointShouldCreateCalculationTask(),
                                       _ => And_TaskShouldBeCreated(),
                                       _ => And_TaskShouldBeCalculated(),
                                       _ => Then_TaskResultShouldBeRetrivedWithPRoperStatus(Dtos.Status.Failed))
                        .RunAsync();
        }
    }
}
