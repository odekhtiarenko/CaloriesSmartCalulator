using AutoFixture;
using CaloriesSmartCalulator.Data.Entities;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CaloriesSmartCalulator.DAL.Tests
{
    public class CaloriesCalculationTaskItemRepositoryTests : RepositoryTestsBase<CaloriesCalculationTaskItem>
    {
        public CaloriesCalculationTaskItemRepositoryTests()
            : base()
        {
            _repository = new CaloriesCalculationTaskItemRepository(_context);
        }

        [Fact]
        public async Task Insert_ShouldAddEntityToDataBaseAndReturnIt()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();

            task.Id = Guid.NewGuid();

            _context.CaloriesCalculationTasks.Add(task);
            _context.SaveChanges();

            var taskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            taskItem.CaloriesCalculationTaskId = task.Id;

            var result = await _repository.InsertAsync(taskItem);

            var expected = _context.CaloriesCalculationTaskItems.Find(taskItem.Id);

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

            var expectedTaskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            expectedTaskItem.CaloriesCalculationTaskId = task.Id;

            _context.CaloriesCalculationTaskItems.Add(expectedTaskItem);
            _context.SaveChanges();

            var result = await _repository.GetAsync(expectedTaskItem.Id);

            result.Should()
                  .NotBeNull()
                  .And
                  .Be(expectedTaskItem);
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

            var expectedTaskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            expectedTaskItem.CaloriesCalculationTaskId = task.Id;

            _context.CaloriesCalculationTaskItems.Add(expectedTaskItem);
            _context.SaveChanges();

            expectedTaskItem.FailedOn = DateTime.Now;
            expectedTaskItem.FinishedOn = DateTime.Now;

            var result = await _repository.UpdateAsync(expectedTaskItem);

            result.Should()
                  .NotBeNull()
                  .And
                  .Be(expectedTaskItem);

            var updatedResult = await _repository.GetAsync(expectedTaskItem.Id);

            updatedResult.Should()
                         .NotBeNull()
                         .And
                         .Be(expectedTaskItem);
        }

        [Fact]
        public async Task Filter_ShouldReturnFilteredEntities()
        {
            var task = _fixture.Create<CaloriesCalculationTask>();

            _context.CaloriesCalculationTasks.Add(task);
            _context.SaveChanges();

            var expectedTaskItems = _fixture.CreateMany<CaloriesCalculationTaskItem>(10);
            foreach (var item in expectedTaskItems)
            {
                item.CaloriesCalculationTaskId = task.Id;
                item.Calories = 10;
            }


            var expectedTaskItem = _fixture.Create<CaloriesCalculationTaskItem>();
            expectedTaskItem.CaloriesCalculationTaskId = task.Id;
            expectedTaskItem.Calories = 100;

            _context.CaloriesCalculationTaskItems.AddRange(expectedTaskItems);
            _context.SaveChanges();


            var result = await _repository.FilterAsync(x=>x.Calories<=10, 10);

            result.Should()
                  .NotBeNull()
                  .And
                  .Contain(x=>x.Calories == 10);
        }
    }
}
