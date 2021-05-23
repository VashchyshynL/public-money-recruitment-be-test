using System;
using System.Threading.Tasks;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRentalsRepository Rentals { get; }
        IBookingsRepository Bookings { get; }
        Task<int> CompleteAsync();
    }
}
