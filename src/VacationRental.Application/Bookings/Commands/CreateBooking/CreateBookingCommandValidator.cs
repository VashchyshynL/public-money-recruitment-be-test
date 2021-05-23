using FluentValidation;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(b => b.RentalId)
                .GreaterThan(0).WithMessage("RentalId should be greater than 0");
        }
    }
}
