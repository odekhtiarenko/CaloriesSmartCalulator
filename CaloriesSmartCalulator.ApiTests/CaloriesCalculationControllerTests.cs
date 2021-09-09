using AutoFixture;
using AutoMapper;
using CaloriesSmartCalulator.Controllers;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Results;
using CaloriesSmartCalulator.MapperProfile;
using FluentAssertions;
using MediatR;
using Moq;
using System;
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
    }
}
