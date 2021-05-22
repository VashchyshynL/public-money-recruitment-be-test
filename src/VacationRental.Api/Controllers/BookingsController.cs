using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using VacationRental.Application.Bookings.Queries.GetBookingById;
using VacationRental.Application.Common.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            return await _mediator.Send(new GetBookingByIdQuery { BookingId = bookingId });
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            return await _mediator.Send(new CreateBookingCommand
            {
                RentalId = model.RentalId, 
                Start = model.Start, 
                Nights = model.Nights
            });
        }
    }
}
