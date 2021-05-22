using MediatR;
using System;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQuery : IRequest<CalendarViewModel>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
