using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCalendarQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarQuery query, CancellationToken cancellationToken)
        {
            var calendar = new CalendarViewModel
            {
                RentalId = query.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var rental = await _unitOfWork.Rentals.GetRentalWithBookings(query.RentalId, cancellationToken);
            if (rental == null)
                throw new NotFoundException("Rental not found");

            for (var i = 0; i < query.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = query.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                var bookings = rental.Bookings
                    .Where(b => b.Start <= date.Date 
                                && b.Start.AddDays(b.Nights) > date.Date)
                    .Select(b => new CalendarBookingViewModel {Id = b.Id});

                date.Bookings.AddRange(bookings);

                calendar.Dates.Add(date);
            }

            return calendar;
        }
    }
}
