using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence.Repositories
{
    public class BookingsRepository : Repository<Booking>, IBookingsRepository
    {
        public BookingsRepository(VacationRentalDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyCollection<Booking>> GetOverlappingBookings(int rentalId, int preparationDays, DateTime startDate, DateTime endDate)
        {
            return await Context.Bookings
                .AsNoTracking()
                .Where(b => 
                    b.RentalId == rentalId 
                    && ((b.Start <= startDate && b.End.AddDays(preparationDays) > startDate)
                        || (b.Start < endDate.AddDays(preparationDays) && b.End.AddDays(preparationDays) >= endDate.AddDays(preparationDays))
                        || (b.Start > startDate && b.End.AddDays(preparationDays) < endDate.AddDays(preparationDays))))
                .ToArrayAsync();
        }
    }
}
