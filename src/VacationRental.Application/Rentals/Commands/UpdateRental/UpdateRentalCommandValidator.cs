using FluentValidation;
using VacationRental.Application.Common.Constants;

namespace VacationRental.Application.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandValidator : AbstractValidator<UpdateRentalCommand>
    {
        public UpdateRentalCommandValidator()
        {
            RuleFor(b => b.RentalId)
                .GreaterThan(0).WithMessage(RentalValidationMessages.PositiveId);

            RuleFor(r => r.Units)
                .GreaterThan(0).WithMessage(RentalValidationMessages.PositiveUnits);

            RuleFor(r => r.PreparationTimeInDays)
                .GreaterThanOrEqualTo(0).WithMessage(RentalValidationMessages.PositivePreparationTime);
        }
    }
}
