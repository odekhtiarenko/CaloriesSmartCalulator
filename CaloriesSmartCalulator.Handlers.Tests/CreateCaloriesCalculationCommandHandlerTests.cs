using AutoFixture;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.CommandHandlers;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.Handlers.Tests
{
    public class CreateCaloriesCalculationCommandHandlerTests
    {
        private readonly CreateCaloriesCalculationCommandHandler _handler;
        private readonly Mock<IRepository<CaloriesCalculationTask>> _repositoryMoq;
        private readonly IFixture _fixture;

        public CreateCaloriesCalculationCommandHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMoq = new Mock<IRepository<CaloriesCalculationTask>>();
            _handler = new CreateCaloriesCalculationCommandHandler(_repositoryMoq.Object);
        }

        [Fact]
        public async Task Handle_ShouldReuturnSuccessfulResult()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();
            _repositoryMoq.Setup(x => x.InsertAsync(It.IsAny<CaloriesCalculationTask>())).ReturnsAsync(task);

            var result = await _handler.Handle(_fixture.Create<CreateCaloriesCalculationCommand>(), new CancellationToken());

            result.Success
                .Should()
                .BeTrue();

            result.Value.Id
                .Should()
                .Be(task.Id);
        }

        [Fact]
        public async Task Handle_ShouldReuturnFailedResultOnException()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();
            var exceptionMessage = "Exception message";
            _repositoryMoq.Setup(x => x.InsertAsync(It.IsAny<CaloriesCalculationTask>())).ThrowsAsync(new System.Exception(exceptionMessage));

            var result = await _handler.Handle(_fixture.Create<CreateCaloriesCalculationCommand>(), new CancellationToken());

            result.Success
                .Should()
                .BeFalse();

            result.FailureMessage
                .Should()
                .Be(exceptionMessage);
        }
    }
}
