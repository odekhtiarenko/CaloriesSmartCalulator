using CaloriesSmartCalulator.Dtos.Requests;
using FluentValidation;

namespace CaloriesSmartCalulator.Validators
{
    public class CalculateMealCaloriesRequestValidator : AbstractValidator<CalculateMealCaloriesRequest>
    {
        public CalculateMealCaloriesRequestValidator()
        {
            RuleFor(req => req.Name)
                .NotEmpty()
                .WithMessage(ValidatorsErrorMessages.NameNotEmptyMessage )
                .MaximumLength(100)
                .WithMessage(ValidatorsErrorMessages.NameMaxLengthMessage);

            RuleFor(req => req.Products)
                .NotEmpty()
                .ForEach(x => x.Matches("^[A-Za-z]+$")
                               .WithMessage(ValidatorsErrorMessages.ProductRegexErrorMessage));
        }
    }
}
