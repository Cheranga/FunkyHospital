using FluentValidation;
using FunkyHospital.Api.DataAccess.Commands;

namespace FunkyHospital.Api.Validators
{
    public class CreateOrderCommandValidator : ModelValidatorBase<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.MobileNumber).NotNull().NotEmpty();
            RuleFor(x => x.PostCode).NotNull().NotEmpty();
        }
    }
}