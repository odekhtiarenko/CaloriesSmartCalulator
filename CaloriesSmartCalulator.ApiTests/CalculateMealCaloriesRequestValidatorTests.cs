using AutoFixture;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Validators;
using FluentAssertions;
using Xunit;

namespace CaloriesSmartCalulator.ApiTests
{
    public class CalculateMealCaloriesRequestValidatorTests
    {
        private readonly Fixture _fixture;
        private readonly CalculateMealCaloriesRequestValidator _validator;

        public CalculateMealCaloriesRequestValidatorTests()
        {
            _fixture = new Fixture();
            _validator = new CalculateMealCaloriesRequestValidator();
        }

        [Fact]
        public void Validator_Validates_Valid()
        {
            var request = new CalculateMealCaloriesRequest();

            request.Name = "Name";
            request.Products = new[] { "product", "meal" };

            var resut = _validator.Validate(request);

            resut.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_Validates_NotValidLongName()
        {
            var request = new CalculateMealCaloriesRequest();

            request.Name = string.Join(string.Empty, _fixture.CreateMany<char>(101));
            request.Products = new[] { "product", "meal" };

            var resut = _validator.Validate(request);

            resut.IsValid.Should().BeFalse();
            resut.Errors.Should().Contain(x => x.ErrorMessage == ValidatorsErrorMessages.NameMaxLengthMessage);
        }

        [Fact]
        public void Validator_Validates_NotValidEmptyName()
        {
            var request = new CalculateMealCaloriesRequest();

            request.Name = string.Empty;
            request.Products = new[] { "product", "meal" };

            var resut = _validator.Validate(request);

            resut.IsValid.Should().BeFalse();
            resut.Errors.Should().Contain(x => x.ErrorMessage == ValidatorsErrorMessages.NameNotEmptyMessage);
        }

        [Fact]
        public void Validator_Validates_NotValidProductsContainNumbers()
        {
            var request = new CalculateMealCaloriesRequest();

            request.Name = "Name";
            request.Products = new[] { "product1", "meal" };

            var resut = _validator.Validate(request);

            resut.IsValid.Should().BeFalse();
            resut.Errors.Should().Contain(x => x.ErrorMessage == ValidatorsErrorMessages.ProductRegexErrorMessage);
        }
    }
}
