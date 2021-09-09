using AutoFixture;
using AutoMapper;
using CaloriesSmartCalulator.Controllers;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
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

        public CaloriesCalculationControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mediatorMoq = new Mock<IMediator>();
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _controller = new CaloriesCalculationController(_mediatorMoq.Object, config.CreateMapper());
        }

        [Fact]
        public async Task CreateCalculationTask_ShouldReturnTaskId()
        {
            var command = _fixture.Create<CreateCaloriesCalculationCommand>();
            var result = new CreateCaloriesCalculationResult(Guid.NewGuid());

            _mediatorMoq.Setup(x => x.Send(It.IsAny<CreateCaloriesCalculationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);
            
            var response = await _controller.CreateCalculationTask(command.Products);
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .Be(result.Value.ToString());
        }

        [Fact]
        public async Task CreateCalculationTask_ShouldReturnFailureMessage()
        {
            var command = _fixture.Create<CreateCaloriesCalculationCommand>();
            var exceptionMessage = "This is exception";
            var result = new CreateCaloriesCalculationResult(new Exception(exceptionMessage));

            _mediatorMoq.Setup(x => x.Send(It.IsAny<CreateCaloriesCalculationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = await _controller.CreateCalculationTask(command.Products);
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .Be(exceptionMessage);
        }

        [Fact]
        public async Task GetCalculationTaskStatus_ShouldCalculationSatus()
        {
            var result = _fixture.Create<CaloriesCalculationTask>();

            _mediatorMoq.Setup(x => x.Send(It.IsAny<GetCaloriesCalculationTaskStatusQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

            var response = (OkObjectResult) await _controller.GetCalculationTaskStatus(Guid.NewGuid());
            
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
