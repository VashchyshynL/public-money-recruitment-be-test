using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence
{
    public class VacationRentalDbContext : DbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
