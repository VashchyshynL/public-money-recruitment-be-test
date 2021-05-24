using FluentValidation;
using VacationRental.Application.Common.Constants;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(b => b.RentalId)
                .GreaterThan(0).WithMessage(RentalValidationMessages.PositiveId);

            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage(BookingValidationMessages.PositiveNights);
        }
    }
}
