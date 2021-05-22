using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Rentals.Queries.GetRentalById
{
    public class GetRentalByIdQueryValidator : AbstractValidator<GetRentalByIdQuery>
    {
        private readonly IVacationRentalDbContext _context;

        public GetRentalByIdQueryValidator(IVacationRentalDbContext context)
        {
            _context = context;

            RuleFor(r => r.RentalId)
                .MustAsync(BeFound).WithMessage("Rental not found");
        }

        private async Task<bool> BeFound(int rentalId, CancellationToken cancellationToken)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            return rental != null;
        }
    }
}
