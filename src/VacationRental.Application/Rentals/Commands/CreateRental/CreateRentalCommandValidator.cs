using FluentValidation;
using VacationRental.Application.Common.Constants;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(r => r.Units)
                .GreaterThan(0).WithMessage(RentalValidationMessages.PositiveUnits);

            RuleFor(r => r.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0).WithMessage(RentalValidationMessages.PositivePreparationTime);
        }
    }
}
