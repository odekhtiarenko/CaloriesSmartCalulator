using CaloriesSmartCalulator.AcceptanceTests.Helpers;
using CaloriesSmartCalulator.Data;
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
            _httpClient = _factory.CreateClient();

            var builder = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.Development.json", true)
                                .AddEnvironmentVariables();


            var options = new DbContextOptionsBuilder<CaloriesCalulatorDBContext>()
                              .UseSqlServer(builder.Build().GetConnectionString("DefaultConnection"))
                              .Options;

            _db = new CaloriesCalulatorDBContext(options);
        }

        private void Given_ProductsToCalculateCalories()
        {
            _products = new[] { "bread", "meat", "cake" };
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
    }
}
