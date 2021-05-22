using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IVacationRentalDbContext
    {
        DbSet<Rental> Rentals { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
