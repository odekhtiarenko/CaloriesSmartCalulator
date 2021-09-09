using AutoFixture;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.CommandHandlers;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.ServiceClients;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.Handlers.Tests
{
    public class CalulteCaloriesForAProductCommandHandlerTests
    {
        private readonly CalulteCaloriesForAProductCommandHandler _handler;

        private readonly Mock<IRepository<CaloriesCalculationTaskItem>> _caloriesCalculationTaskItemsRespositoryMoq;
        private readonly Mock<ILogger<CalulteCaloriesForAProductCommandHandler>> _loggerMoq;
        private readonly Mock<IMediator> _mediatorMoq;
        private readonly Mock<ICaloriesServiceClient> _caloriesServiceClientMoq;
        private Fixture _fixture;

        public CalulteCaloriesForAProductCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _caloriesCalculationTaskItemsRespositoryMoq = new Mock<IRepository<CaloriesCalculationTaskItem>>();
            _caloriesServiceClientMoq = new Mock<ICaloriesServiceClient>();
            _mediatorMoq = new Mock<IMediator>();
            _loggerMoq = new Mock<ILogger<CalulteCaloriesForAProductCommandHandler>>();

            _handler = new CalulteCaloriesForAProductCommandHandler(_caloriesCalculationTaskItemsRespositoryMoq.Object, 
                                                                    _loggerMoq.Object,
                                                                    _mediatorMoq.Object,
                                                                    _caloriesServiceClientMoq.Object);
        }

        [Fact]
        public async Task Handle_Command_ShouldCalculateSuccesful()
        {
            var command = _fixture.Create<CalulteCaloriesForAProductCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            var unfinishedTaskItem = _fixture.Create<CaloriesCalculationTaskItem>();

            unfinishedTaskItem.FinishedOn = null;
            unfinishedTaskItem.FailedOn = null;

            taskItem.CaloriesCalculationTask.CaloriesCalculationTaskItems.Add(unfinishedTaskItem);

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync(taskItem);

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.UpdateAsync(It.IsAny<CaloriesCalculationTaskItem>()))
                .ReturnsAsync(taskItem);

            _caloriesServiceClientMoq.Setup(x => x.GetCaloriesForProduct(taskItem.Product))
                                     .ReturnsAsync(10);
            
            var result = await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Never);

            result.Success
                  .Should()
                  .BeTrue();
        }

        [Fact]
        public async Task Handle_Command_ShouldCalculateFailed()
        {
            var command = _fixture.Create<CalulteCaloriesForAProductCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            var message = "Exception message";

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync(taskItem);

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.UpdateAsync(It.IsAny<CaloriesCalculationTaskItem>()))
                .ReturnsAsync(taskItem);

            _caloriesServiceClientMoq.Setup(x => x.GetCaloriesForProduct(taskItem.Product))
                                     .ThrowsAsync(new System.Exception(message));

            var result = await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            result.Success
                  .Should()
                  .BeFalse();
        }

        [Fact]
        public async Task Handle_Command_ShouldNullTaskItem()
        {
            var command = _fixture.Create<CalulteCaloriesForAProductCommand>();

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync(default(CaloriesCalculationTaskItem));

            var result = await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Never);

            result.Success
                  .Should()
                  .BeFalse();
        }

        [Fact]
        public async Task Handle_Command_ShouldCalculateSuccesfulAndCloseTask()
        {
            var command = _fixture.Create<CalulteCaloriesForAProductCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTaskItem>();

            foreach (var item in taskItem.CaloriesCalculationTask.CaloriesCalculationTaskItems)
            {
                item.FailedOn = DateTime.Now;
                item.FinishedOn = DateTime.Now;
            }

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync(taskItem);

            _caloriesCalculationTaskItemsRespositoryMoq.Setup(x => x.UpdateAsync(It.IsAny<CaloriesCalculationTaskItem>()))
                .ReturnsAsync(taskItem);

            _caloriesServiceClientMoq.Setup(x => x.GetCaloriesForProduct(taskItem.Product))
                                     .ReturnsAsync(10);

            var result = await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Success
                  .Should()
                  .BeTrue();
        }
    }
}
