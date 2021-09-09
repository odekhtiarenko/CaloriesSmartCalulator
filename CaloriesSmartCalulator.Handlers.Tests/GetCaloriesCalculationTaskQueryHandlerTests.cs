using AutoFixture;
using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.Contracts.Queries;
using CaloriesSmartCalulator.Handlers.QueryHandlers;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.Handlers.Tests
{
    public class GetCaloriesCalculationTaskQueryHandlerTests
    {
        private readonly GetCaloriesCalculationTaskQueryHandler _handler;
        private readonly Mock<IRepository<CaloriesCalculationTask>> _repositoryMoq;
        private readonly IFixture _fixture;

        public GetCaloriesCalculationTaskQueryHandlerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repositoryMoq = new Mock<IRepository<CaloriesCalculationTask>>();
            _handler = new GetCaloriesCalculationTaskQueryHandler(_repositoryMoq.Object);
        }

        [Fact]
        public async Task Handle_GetCaloriesCalculationTaskResultQuery_ShouldReuturnCalculationTask()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();
            var query = _fixture.Create<GetCaloriesCalculationTaskResultQuery>();

            _repositoryMoq.Setup(x => x.GetAsync(query.Id)).ReturnsAsync(task);

            var result = await _handler.Handle(query, new CancellationToken());
            result.Should()
                .NotBeNull()
                .And
                .Be(task);
        }

        [Fact]
        public async Task Handle_GetCaloriesCalculationTaskStatusQuery_ShouldReuturnCalculationTask()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();
            var query = _fixture.Create<GetCaloriesCalculationTaskResultQuery>();

            _repositoryMoq.Setup(x => x.GetAsync(query.Id)).ReturnsAsync(task);

            var result = await _handler.Handle(query, new CancellationToken());
            result.Should()
                .NotBeNull()
                .And
                .Be(task);
        }

        [Fact]
        public async Task Handle_GetCaloriesCalculationTaskResultQuery_ShouldReuturnNullIfTaskDoesntExists()
        {
            _repositoryMoq.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(CaloriesCalculationTask));

            var result = await _handler.Handle(_fixture.Create<GetCaloriesCalculationTaskResultQuery>(), new CancellationToken());

            result.Should()
                  .BeNull();
        }

        [Fact]
        public async Task Handle_GetCaloriesCalculationTaskStatusQuery_ShouldReuturnNullIfTaskDoesntExists()
        {
            _repositoryMoq.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(default(CaloriesCalculationTask));

            var result = await _handler.Handle(_fixture.Create<GetCaloriesCalculationTaskStatusQuery>(), new CancellationToken());

            result.Should()
                  .BeNull();
        }
    }
}
