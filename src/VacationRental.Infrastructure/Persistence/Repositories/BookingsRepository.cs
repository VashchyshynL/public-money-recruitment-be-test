using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence.Repositories
{
    public class BookingsRepository : Repository<Booking>, IBookingsRepository
    {
        public BookingsRepository(VacationRentalDbContext context) : base(context)
        {

        }

        public async Task<bool> IsExists(int rentalId, CancellationToken cancellationToken)
        {
            var rental = await GetByIdAsync(rentalId);
            return rental != null;
        }
    }
}
