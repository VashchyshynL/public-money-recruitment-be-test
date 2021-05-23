using System.Threading;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IRentalsRepository
    {
        Task<Rental> GetByIdAsync(int rentalId);
        Task<Rental> GetRentalWithBookings(int rentalId, CancellationToken cancellationToken);
        Task<bool> IsExists(int rentalId, CancellationToken cancellationToken);
        Task AddAsync(Rental rental);
    }
}
