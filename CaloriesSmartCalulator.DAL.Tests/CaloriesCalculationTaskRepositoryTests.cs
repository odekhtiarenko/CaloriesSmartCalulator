using AutoFixture;
using CaloriesSmartCalulator.Data.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.DAL.Tests
{
    public class CaloriesCalculationTaskRepositoryTests : RepositoryTestsBase<CaloriesCalculationTask>
    {
        public CaloriesCalculationTaskRepositoryTests()
            : base()
        {
            _repository = new CaloriesCalculationTaskRepository(_context);
        }

        [Fact]
        public async Task Insert_ShouldAddEntityToDataBaseAndReturnIt()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();

            var result = await _repository.InsertAsync(task);

            var expected = _context.CaloriesCalculationTasks.Find(task.Id);

            result.Should()
                  .NotBeNull()
                  .And
                  .Be(expected);
        }

        [Fact]
        public async Task Get_ShouldReturnEntity()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();

            _context.CaloriesCalculationTasks.Add(task);
            _context.SaveChanges();

            var result = await _repository.GetAsync(task.Id);

            result.Should()
                  .NotBeNull()
                  .And
                  .Be(task);
        }

        [Fact]
        public async Task Get_ShouldReturnNullForNonExistingEntity()
        {
            var result = await _repository.GetAsync(Guid.NewGuid());

            result.Should()
                  .BeNull();
        }

        [Fact]
        public async Task Update_ShouldUpdateEntityReturnEntity()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();

            _context.CaloriesCalculationTasks.Add(task);
            _context.SaveChanges();

            task.FailedOn = DateTime.Now;
            task.FinishedOn = DateTime.Now;

            var result = await _repository.UpdateAsync(task);

            result.Should()
                  .NotBeNull()
                  .And
                  .Be(task);

            var updatedResult = await _repository.GetAsync(task.Id);

            updatedResult.Should()
                         .NotBeNull()
                         .And
                         .Be(task);
        }

        [Fact]
        public async Task Filter_ShouldReturnFilteredEntities()
        {
            var tasks = _fixture.CreateMany<CaloriesCalculationTask>(10);
            foreach (var item in tasks)
            {
                item.FinishedOn = null;
            }
            _context.CaloriesCalculationTasks.AddRange(tasks);

            var task = _fixture.Create<CaloriesCalculationTask>();
            task.FinishedOn = new DateTime(2021, 11, 6);

            _context.CaloriesCalculationTasks.AddRange(task);
            _context.SaveChanges();


            var result = await _repository.FilterAsync(x => x.FinishedOn == null, 10);

            result.Should()
                  .NotBeNull()
                  .And
                  .Contain(x => x.FinishedOn == null);
        }
    }
}
