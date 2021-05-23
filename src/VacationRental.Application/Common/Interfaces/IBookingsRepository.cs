using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IBookingsRepository
    {
        Task<Booking> GetByIdAsync(int bookingId);
        Task<IReadOnlyCollection<Booking>> GetOverlappingBookings(int rentalId, int preparationDays, DateTime startDate, DateTime endDate);
        
        Task AddAsync(Booking booking);
    }
}
