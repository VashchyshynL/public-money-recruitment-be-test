using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<BookingViewModel>
    {
        public int BookingId { get; set; }
    }
}
