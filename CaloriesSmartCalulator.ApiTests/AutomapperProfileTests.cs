using AutoFixture;
using AutoMapper;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.MapperProfile;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CaloriesSmartCalulator.ApiTests
{
    public class AutomapperProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public AutomapperProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();


            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_StatusObject()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        Product = "salt"
                    },
                }
            };

            var expected = new StatusObject()
            {
                Name = "Name",
                Total = 100,
                Percentage = 50,
                Products = new[] { "cake", "meat", "bread" },
                Status = Status.InProgress
            };

            var result = _mapper.Map<StatusObject>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_StatusObject_Finished()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        FinishedOn = DateTime.Now,
                        Calories=20,
                        Product = "salt"
                    },
                }
            };

            var expected = new StatusObject()
            {
                Name = "Name",
                Total = 160,
                Percentage = 100,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.Completed
            };

            var result = _mapper.Map<StatusObject>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_StatusObject_Failed()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        FailedOn = DateTime.Now,
                        Calories=0,
                        Product = "salt"
                    },
                }
            };

            var expected = new StatusObject()
            {
                Name = "Name",
                Total = 140,
                Percentage = 100,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.Failed
            };

            var result = _mapper.Map<StatusObject>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_TaskResult()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                InProgressOn = DateTime.Now,
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=0,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=0,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=0,
                        Product = "salt"
                    },
                }
            };

            var expected = new CalculationTaskResult()
            {
                Name = "Name",
                Total = 100,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.InProgress
            };

            var result = _mapper.Map<CalculationTaskResult>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_TaskResult_Finished()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                InProgressOn = DateTime.Now,
                FinishedOn = DateTime.Now,
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        FinishedOn = DateTime.Now,
                        Calories=20,
                        Product = "salt"
                    },
                }
            };

            var expected = new CalculationTaskResult()
            {
                Name = "Name",
                Total = 160,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.Completed
            };

            var result = _mapper.Map<CalculationTaskResult>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_TaskResult_Failed()
        {
            var input = new CaloriesCalculationTask()
            {
                Name = "Name",
                InProgressOn = DateTime.Now,
                FailedOn = DateTime.Now,
                CaloriesCalculationTaskItems = new List<CaloriesCalculationTaskItem>()
                {
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FailedOn = DateTime.Now,
                        Product = "cake"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "meat"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=40,
                        FinishedOn = DateTime.Now,
                        Product = "bread"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "cow"
                    },
                    new CaloriesCalculationTaskItem(){
                        Calories=20,
                        FinishedOn = DateTime.Now,
                        Product = "onion"
                    },
                    new CaloriesCalculationTaskItem(){
                        FinishedOn = DateTime.Now,
                        Calories=20,
                        Product = "salt"
                    },
                }
            };

            var expected = new CalculationTaskResult()
            {
                Name = "Name",
                Total = 160,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.Failed
            };

            var result = _mapper.Map<CalculationTaskResult>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }

        [Fact]
        public void Map_CalculateMealCaloriesRequest_To_CaloriesCalculation()
        {
            var input = _fixture.Create<CalculateMealCaloriesRequest>();
            var expected = new CreateCaloriesCalculationCommand();

            expected.Name = input.Name;
            expected.Products = input.Products;

            var result = _mapper.Map<CreateCaloriesCalculationCommand>(input);

            result.Should()
            .BeEquivalentTo(expected);
        }
    }
}
