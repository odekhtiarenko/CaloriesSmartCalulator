using AutoFixture;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.CommandHandlers;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.Handlers.Tests
{
    public class FinishCalculationTaskCommandHandlerTests
    {
        private readonly FinishCalculationTaskCommandHandler _handler;

        private readonly Mock<IRepository<CaloriesCalculationTask>> _caloriesCalculationTaskRespositoryMoq;
        private readonly Mock<IMediator> _mediatorMoq;
        private readonly Mock<ILogger<FinishCalculationTaskCommandHandler>> _loggerMoq;
        private readonly Fixture _fixture;

        public FinishCalculationTaskCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _caloriesCalculationTaskRespositoryMoq = new Mock<IRepository<CaloriesCalculationTask>>();
            _loggerMoq = new Mock<ILogger<FinishCalculationTaskCommandHandler>>();
            _mediatorMoq = new Mock<IMediator>();

            _handler = new FinishCalculationTaskCommandHandler(_caloriesCalculationTaskRespositoryMoq.Object,
                                                               _mediatorMoq.Object,
                                                               _loggerMoq.Object);
        }

        [Fact]
        public async Task Hadle_Command_ShouldResendCommanIfTaskHasUnFinishedItems()
        {
            var command = _fixture.Create<FinishCalculationTaskCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTask>();

            var unfinishedTaskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            unfinishedTaskItem.FinishedOn = null;
            unfinishedTaskItem.FailedOn = null;
            taskItem.CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>();
            taskItem.CaloriesCalculationTaskItems.Add(unfinishedTaskItem);

            _caloriesCalculationTaskRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                                                  .ReturnsAsync(taskItem);

            await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _caloriesCalculationTaskRespositoryMoq.Verify(x => x.UpdateAsync(It.IsAny<CaloriesCalculationTask>()), Times.Never);
        }

        [Fact]
        public async Task Hadle_Command_ShouldResendCommanIfTaskHasFinishedItemsUpdateFinish()
        {
            var command = _fixture.Create<FinishCalculationTaskCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTask>();

            foreach (var item in taskItem.CaloriesCalculationTaskItems)
            {
                item.FinishedOn = DateTime.Now;
                item.FailedOn = null;
            }

            _caloriesCalculationTaskRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                                                  .ReturnsAsync(taskItem);

            await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _caloriesCalculationTaskRespositoryMoq.Verify(x => x.UpdateAsync(It.Is<CaloriesCalculationTask>(x => x.FinishedOn != null)), Times.Once);
        }

        [Fact]
        public async Task Hadle_Command_ShouldResendCommanIfTaskHasFinishedItemsUpdateFailed()
        {
            var command = _fixture.Create<FinishCalculationTaskCommand>();
            var taskItem = _fixture.Create<CaloriesCalculationTask>();

            foreach (var item in taskItem.CaloriesCalculationTaskItems)
            {
                item.FinishedOn = null;
                item.FailedOn = DateTime.Now;
            }

            _caloriesCalculationTaskRespositoryMoq.Setup(x => x.GetAsync(command.Id))
                                                  .ReturnsAsync(taskItem);

            await _handler.Handle(command, new CancellationToken());

            _mediatorMoq.Verify(x => x.Send(It.IsAny<FinishCalculationTaskCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _caloriesCalculationTaskRespositoryMoq.Verify(x => x.UpdateAsync(It.Is<CaloriesCalculationTask>(x => x.FailedOn != null)), Times.Once);
        }
    }
}
