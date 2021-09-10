using AutoFixture;
using AutoMapper;
using CaloriesSmartCalulator.Controllers;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Queries;
using CaloriesSmartCalulator.Handlers.Contracts.Results;
using CaloriesSmartCalulator.MapperProfile;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.ApiTests
{
    public class CaloriesCalculationControllerTests
    {
        private readonly CaloriesCalculationController _controller;
        private readonly Mock<IMediator> _mediatorMoq;
        private readonly IFixture _fixture;

        private CalculateMealCaloriesRequest _request;

        public CaloriesCalculationControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMoq = new Mock<IMediator>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _controller = new CaloriesCalculationController(_mediatorMoq.Object, config.CreateMapper());
        }

        [Fact]
        public async Task CreateCalculationTask_ShouldReturnTaskId()
        {
            _request = new CalculateMealCaloriesRequest() { Name = "Name", Products = new[] { "test", "testA" } };
            var result = new CreateCaloriesCalculationResult(_fixture.Create<CaloriesCalculationTask>());

            _mediatorMoq.Setup(x => x.Send(It.IsAny<CreateCaloriesCalculationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = (OkObjectResult)await _controller.CreateCalculationTask(_request);
            response.Should()
                .NotBeNull()
                .And
                .BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async Task CreateCalculationTask_ShouldReturnFailureMessage()
        {
            _request = new CalculateMealCaloriesRequest() { Name = "Name", Products = new[] { "test", "testA" } };
            var exceptionMessage = "This is exception";
            var result = new CreateCaloriesCalculationResult(new Exception(exceptionMessage));

            _mediatorMoq.Setup(x => x.Send(It.IsAny<CreateCaloriesCalculationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.CreateCalculationTask(_request);
            response.Should()
                .NotBeNull()
                .And
                .BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task GetCalculationTaskStatus_ShouldCalculationSatus()
        {
            var result = _fixture.Create<CaloriesCalculationTask>();

            _mediatorMoq.Setup(x => x.Send(It.IsAny<GetCaloriesCalculationTaskStatusQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = (OkObjectResult)await _controller.GetCalculationTaskStatus(Guid.NewGuid());

            response.Value
                    .Should()
                    .BeOfType(typeof(StatusObject));
        }

        [Fact]
        public async Task GetCalculationTaskResult_ShouldCalculatioResult()
        {
            var result = _fixture.Create<CaloriesCalculationTask>();

            _mediatorMoq.Setup(x => x.Send(It.IsAny<GetCaloriesCalculationTaskResultQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = (OkObjectResult)await _controller.GetTaskResult(Guid.NewGuid());

            response.Value
                    .Should()
                    .BeOfType(typeof(CalculationTaskResult));
        }
    }
}
