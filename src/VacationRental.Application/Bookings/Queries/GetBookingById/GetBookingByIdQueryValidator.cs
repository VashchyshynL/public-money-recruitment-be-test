using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
    {
        private readonly IVacationRentalDbContext _context;

        public GetBookingByIdQueryValidator(IVacationRentalDbContext context)
        {
            _context = context;

            RuleFor(b => b.BookingId)
                .MustAsync(BookingExists).WithMessage("Booking not found");
        }

        private async Task<bool> BookingExists(int bookingId, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            return booking != null;
        }
    }
}
