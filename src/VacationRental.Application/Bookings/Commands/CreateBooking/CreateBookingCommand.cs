using System;
using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<ResourceIdViewModel>
    {
        public int RentalId { get; set; }
        public int Nights { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddDays(Nights).Date;
    }
}
