using CaloriesSmartCalulator.AcceptanceTests.Helpers;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Dtos.Responses;
using FluentAssertions;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.AcceptanceTests.Features
{
    public partial class CaloriesCalulationFeature : FeatureFixture,
                                                     IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;
        private readonly CaloriesCalulatorDBContext _db;

        #region TestData
        private CalculateMealCaloriesRequest _meal;
        private CalculateMealCaloriesResponse _task;
        #endregion

        public CaloriesCalulationFeature(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;

            var options = new DbContextOptionsBuilder<CaloriesCalulatorDBContext>()
                        .UseInMemoryDatabase(databaseName: "DataBase")
                        .Options;
            _db = new CaloriesCalulatorDBContext(options);

            _httpClient = _factory.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
                   {
                       var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<CaloriesCalulatorDBContext>));

                       services.Remove(descriptor);

                       services.AddDbContext<CaloriesCalulatorDBContext>(options =>
                       {
                           options.UseInMemoryDatabase("DataBase");
                       });

                   })
            ).CreateClient();
        }

        private void Given_ProductsToCalculateCalories()
        {
            _meal = new CalculateMealCaloriesRequest();
            _meal.Products = new[] { "rice", "meat", "cake" };
            _meal.Name = "Breakfast";
        }

        private void Given_FailedProductsToCalculateCalories()
        {
            _meal = new CalculateMealCaloriesRequest();
            _meal.Products = new[] { "exception", "meat", "cake" };
            _meal.Name = "Breakfast";
        }

        private async Task Then_CallToEndpointShouldCreateCalculationTask()
        {
            var response = await _httpClient.PostWithExpectedStatusCodeAsync($"/api/caloriescalculation/create", _meal, HttpStatusCode.OK);
            _task = await response.Content.DeserializeAsync<CalculateMealCaloriesResponse>();

            _task.Id.Should()
                .NotBeEmpty();
        }
        private async Task And_TaskShouldBeCreated()
        {
            var task = await _db.CaloriesCalculationTasks.Include(t => t.CaloriesCalculationTaskItems)
                                                         .FirstOrDefaultAsync(x => x.Id == _task.Id);

            task.Should()
                .NotBeNull();

            task.CaloriesCalculationTaskItems
                .Should()
                .Contain(x => _meal.Products.Contains(x.Product));
        }

        private async Task And_TaskShouldBeCalculated()
        {
            StatusObject taskStatus;
            int i = 0;
            do
            {
                var response = await _httpClient.GetWithExpectedStatusCodeAsync($"/api/caloriescalculation/status/{_task.Id}", HttpStatusCode.OK);
                taskStatus = await response.Content.DeserializeAsync<StatusObject>();
                await Task.Delay(1000);
            } while (taskStatus.Status == Status.InProgress && taskStatus.Percentage < 100 && i < 30);

            taskStatus.Percentage.Should().Be(100);
        }

        private async Task Then_TaskResultShouldBeRetrivedWithPRoperStatus(Status status)
        {
            var response = await _httpClient.GetWithExpectedStatusCodeAsync($"/api/caloriescalculation/{_task.Id}", HttpStatusCode.OK);
            var taskStatus = await response.Content.DeserializeAsync<CalculationTaskResult>();

            taskStatus.Status.Should().Be(status);
        }
    }
}
