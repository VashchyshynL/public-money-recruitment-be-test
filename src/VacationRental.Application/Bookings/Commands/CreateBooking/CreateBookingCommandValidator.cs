using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        private readonly IVacationRentalDbContext _context;

        public CreateBookingCommandValidator(IVacationRentalDbContext context)
        {
            _context = context;

            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(b => b.RentalId)
                .MustAsync(RentalExists).WithMessage("Rental not found");

            RuleFor(b => b)
                .MustAsync(BookingAvailable).WithMessage("Not available");
        }

        private async Task<bool> RentalExists(int rentalId, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            return rental != null;
        }

        private async Task<bool> BookingAvailable(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var count = _context.Bookings.Count(b => b.RentalId == command.RentalId && (b.Start <= command.Start.Date &&
                                                         b.Start.AddDays(b.Nights) >
                                                         command.Start.Date) ||
                                                     (b.Start < command.Start.AddDays(command.Nights) && b.Start.AddDays(b.Nights) >=
                                                         command.Start.AddDays(command.Nights)) || (b.Start > command.Start &&
                                                         b.Start.AddDays(b.Nights) <
                                                         command.Start.AddDays(command.Nights)));

            var rental = await _context.Rentals.FindAsync(command.RentalId);

            if (count >= rental.Units)
                throw new ApplicationException("Not available");

            return true;
        }
    }
}
