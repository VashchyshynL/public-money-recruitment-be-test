using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

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

            var rental = await _unitOfWork.Rentals.GetByIdAsync(query.RentalId);
            if (rental == null)
                throw new ValidationException(nameof(query.RentalId), "Rental not found");

            var overlappingBookings = await _unitOfWork.Bookings.GetOverlappingBookings(
                rental.Id, rental.PreparationTimeInDays, query.Start, query.Start.AddDays(query.Nights));

            for (var i = 0; i < query.Nights; i++)
            {
                var calendarDate = new CalendarDateViewModel
                {
                    Date = query.Start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<CalendarPreparationTimeViewModel>()
                };

                var calendarDateBookings = GetCalendarDateBookings(overlappingBookings, calendarDate.Date);
                calendarDate.Bookings.AddRange(calendarDateBookings);

                var preparationTimes = GetPreparationTimeUnits(overlappingBookings, calendarDate.Date,
                    rental.PreparationTimeInDays);
                calendarDate.PreparationTimes.AddRange(preparationTimes);

                calendar.Dates.Add(calendarDate);
            }

            return calendar;
        }

        private static IEnumerable<CalendarBookingViewModel> GetCalendarDateBookings(
            IEnumerable<Booking> overlappingBookings, DateTime calendarDate)
        {
            return overlappingBookings
                .Where(b => b.Start <= calendarDate && b.End > calendarDate)
                .Select(b => new CalendarBookingViewModel {Id = b.Id, Unit = b.Unit});
        }

        private static IEnumerable<CalendarPreparationTimeViewModel> GetPreparationTimeUnits(
            IEnumerable<Booking> overlappingBookings, DateTime calendarDate, int preparationDays)
        {
            return overlappingBookings
                .Where(b => b.End <= calendarDate && b.End.AddDays(preparationDays) > calendarDate)
                .Select(b => new CalendarPreparationTimeViewModel { Unit = b.Unit });
        }
    }
}
