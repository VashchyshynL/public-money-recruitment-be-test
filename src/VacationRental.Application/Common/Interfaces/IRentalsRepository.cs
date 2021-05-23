using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IRentalsRepository
    {
        Task<Rental> GetByIdAsync(int rentalId);

        Task AddAsync(Rental rental);
    }
}
