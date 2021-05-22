using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarViewModel>
    {
        private readonly IVacationRentalDbContext _context;

        public GetCalendarQueryHandler(IVacationRentalDbContext context)
        {
            _context = context;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarQuery query, CancellationToken cancellationToken)
        {
            var calendar = new CalendarViewModel
            {
                RentalId = query.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < query.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = query.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                var bookings = await _context.Bookings
                    .Where(b => 
                        b.RentalId == query.RentalId
                        && b.Start <= date.Date 
                        && b.Start.AddDays(b.Nights) > date.Date)
                    .Select(b => new CalendarBookingViewModel {Id = b.Id})
                    .ToArrayAsync(cancellationToken: cancellationToken);

                date.Bookings.AddRange(bookings);

                calendar.Dates.Add(date);
            }

            return calendar;
        }
    }
}
