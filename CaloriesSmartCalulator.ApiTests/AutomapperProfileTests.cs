using AutoMapper;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.MapperProfile;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CaloriesSmartCalulator.ApiTests
{
    public class AutomapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutomapperProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CaloriesCalculationTask_To_StatusObject()
        {
            var input = new CaloriesCalculationTask()
            {
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
                Total = 160,
                Products = new[] { "cake", "meat", "bread", "cow", "onion", "salt" },
                Status = Status.Failed
            };

            var result = _mapper.Map<CalculationTaskResult>(input);

            result.Should()
                  .BeEquivalentTo(expected);
        }
    }
}
