using System.Threading.Tasks;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Infrastructure.Persistence.Repositories;

namespace VacationRental.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VacationRentalDbContext _context;

        public UnitOfWork(VacationRentalDbContext context)
        {
            _context = context;
            Rentals = new RentalsRepository(_context);
            Bookings = new BookingsRepository(_context);
        }

        public IRentalsRepository Rentals { get; private set; }
        public IBookingsRepository Bookings { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
