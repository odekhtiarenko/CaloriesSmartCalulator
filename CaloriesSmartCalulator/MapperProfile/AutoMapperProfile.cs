using AutoMapper;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Dtos.Responses;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using System.Collections.Generic;
using System.Linq;

namespace CaloriesSmartCalulator.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CaloriesCalculationTask, StatusObject>()
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(x => CalculatePercentage(x.CaloriesCalculationTaskItems)))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(x => x.CaloriesCalculationTaskItems
                                                                           .Where(x => x.FailedOn != null || x.FinishedOn != null)
                                                                           .Select(t => t.Product)))
               .ForMember(dest => dest.Total, opt => opt.MapFrom(x => x.CaloriesCalculationTaskItems.Where(x => x.FinishedOn != null).Sum(t => t.Calories)))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(x => GetTaskStatus(x.CaloriesCalculationTaskItems)));

            CreateMap<CaloriesCalculationTask, CalculationTaskResult>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(x => GetTaskStatus(x)))
               .ForMember(dest => dest.Products, opt => opt.MapFrom(x => x.CaloriesCalculationTaskItems.Select(t => t.Product)))
               .ForMember(dest => dest.Total, opt => opt.MapFrom(x => x.CaloriesCalculationTaskItems.Sum(t => t.Calories)));

            CreateMap<CalculateMealCaloriesRequest, CreateCaloriesCalculationCommand>();
            CreateMap<CaloriesCalculationTask, CalculateMealCaloriesResponse>();
        }

        private Status GetTaskStatus(ICollection<CaloriesCalculationTaskItem> caloriesCalculationTaskItems)
        {
            if (caloriesCalculationTaskItems.Any(x => !x.FinishedOn.HasValue && !x.FailedOn.HasValue))
                return Status.InProgress;

            if (caloriesCalculationTaskItems.All(x => x.FinishedOn.HasValue))
                return Status.Completed;

            return Status.Failed;
        }

        private Status GetTaskStatus(CaloriesCalculationTask caloriesCalculationTask)
        {
            if (caloriesCalculationTask.FailedOn != null)
                return Status.Failed;

            if (caloriesCalculationTask.FinishedOn != null)
                return Status.Completed;

            return Status.InProgress;
        }

        private int CalculatePercentage(ICollection<CaloriesCalculationTaskItem> caloriesCalculationTaskItems)
        {
            var finished = (decimal)caloriesCalculationTaskItems.Count(x => x.FinishedOn != null || x.FailedOn != null);
            var total = caloriesCalculationTaskItems.Count();

            return (int)(finished / total * 100);
        }
    }
}
