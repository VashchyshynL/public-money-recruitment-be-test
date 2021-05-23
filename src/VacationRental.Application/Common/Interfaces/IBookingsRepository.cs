using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IBookingsRepository
    {
        Task<Booking> GetByIdAsync(int bookingId);
        Task AddAsync(Booking booking);
    }
}
