using Application.RequestMappers.Dtos;
using FluentValidation;

namespace Application.RequestMappers.Validators
{
    public class AddressDtoValidator: AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
        }
    }
}
