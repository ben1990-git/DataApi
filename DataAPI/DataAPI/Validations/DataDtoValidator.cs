using DataAPI.DTOs;
using FluentValidation;

namespace DataAPI.Validations
{
    public class UpdateDataDtoRequestValidator : AbstractValidator<UpdateDataDtoRequest>
    {
        public UpdateDataDtoRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than zero.");

        }
    }
    public class CreateDataDtoRequestValidator : AbstractValidator<CreateDataDtoRequest>
    {
        public CreateDataDtoRequestValidator()
        {
            RuleFor(x => x.Value)
           .NotEmpty()
           .WithMessage("data value cannot be empty");
        }
    }
}
