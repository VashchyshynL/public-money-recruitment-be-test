using FluentValidation;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(r => r.Units)
                .GreaterThan(0).WithMessage("Number of units should be greater that 0");

            RuleFor(r => r.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0).WithMessage("Preparation time in days should be greater or equal to 0");
        }
    }
}
