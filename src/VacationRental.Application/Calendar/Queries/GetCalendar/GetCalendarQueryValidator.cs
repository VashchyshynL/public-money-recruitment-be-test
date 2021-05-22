using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
    {
        private readonly IVacationRentalDbContext _context;

        public GetCalendarQueryValidator(IVacationRentalDbContext context)
        {
            _context = context;

            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(r => r.RentalId)
                .MustAsync(RentalExists).WithMessage("Rental not found");
        }

        private async Task<bool> RentalExists(int rentalId, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            return rental != null;
        }
    }
}
