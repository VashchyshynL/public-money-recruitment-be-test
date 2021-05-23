using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence.Repositories
{
    public class RentalsRepository : Repository<Rental>, IRentalsRepository
    {
        public RentalsRepository(VacationRentalDbContext context) : base(context)
        {
            
        }
        public async Task<bool> IsExists(int rentalId, CancellationToken cancellationToken)
        {
            var rental = await GetByIdAsync(rentalId);
            return rental != null;
        }

        public async Task<Rental> GetRentalWithBookings(int rentalId, CancellationToken cancellationToken)
        {
            return await VacationRentalDbContext.Rentals
                .AsNoTracking()
                .Where(r => r.Id == rentalId)
                .Include(x => x.Bookings)
                .FirstOrDefaultAsync(cancellationToken);
        }

        private VacationRentalDbContext VacationRentalDbContext => Context as VacationRentalDbContext;
    }
}
