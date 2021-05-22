using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence
{
    public class VacationRentalDbContext : DbContext, IVacationRentalDbContext
    {
        public VacationRentalDbContext(DbContextOptions<VacationRentalDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Rental> Rentals { get; set; }
    }
}
