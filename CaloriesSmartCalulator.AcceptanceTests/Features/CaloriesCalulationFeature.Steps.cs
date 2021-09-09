using CaloriesSmartCalulator.AcceptanceTests.Helpers;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
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
        private string[] _products;
        private string _taskId;
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
            _products = new[] { "bread", "meat", "cake" };
        }

        private void Given_FailedProductsToCalculateCalories()
        {
            _products = new[] { "exception", "meat", "cake" };
        }

        private async Task Then_CallToEndpointShouldCreateCalculationTask()
        {
            var response = await _httpClient.PostWithExpectedStatusCodeAsync($"/api/caloriescalculation/create", _products, HttpStatusCode.OK);
            _taskId = await response.Content.ReadAsStringAsync();

            _taskId.Should()
                .NotBeEmpty();
        }
        private async Task And_TaskShouldBeCreated()
        {
            var task = await _db.CaloriesCalculationTasks.Include(t => t.CaloriesCalculationTaskItems)
                                                         .FirstOrDefaultAsync(x => x.Id == Guid.Parse(_taskId));

            task.Should()
                .NotBeNull();

            task.CaloriesCalculationTaskItems
                .Should()
                .Contain(x => _products.Contains(x.Product));
        }

        private async Task And_TaskShouldBeCalculated()
        {
            StatusObject taskStatus;
            int i = 0;
            do
            {
                var response = await _httpClient.GetWithExpectedStatusCodeAsync($"/api/caloriescalculation/status/{_taskId}", HttpStatusCode.OK);
                taskStatus = await response.Content.DeserializeAsync<StatusObject>();
                await Task.Delay(1000);
            } while (taskStatus.Status == Status.InProgress && taskStatus.Percentage < 100 && i < 30);

            taskStatus.Percentage.Should().Be(100);
        }

        private async Task Then_TaskResultShouldBeRetrivedWithPRoperStatus(Status status)
        {
            var response = await _httpClient.GetWithExpectedStatusCodeAsync($"/api/caloriescalculation/{_taskId}", HttpStatusCode.OK);
            var taskStatus = await response.Content.DeserializeAsync<CalculationTaskResult>();

            taskStatus.Status.Should().Be(status);
        }
    }
}
