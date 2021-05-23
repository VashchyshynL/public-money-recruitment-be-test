using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId)
        {
            var booking = await _mediator.Send(new GetBookingByIdQuery { BookingId = bookingId });
            if (booking == null)
                return NotFound("Booking not found");

            return Ok(booking);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel model)
        {
            var booking = await _mediator.Send(new CreateBookingCommand
            {
                RentalId = model.RentalId,
                Start = model.Start,
                Nights = model.Nights
            });
            
            return CreatedAtAction(nameof(Get), new { bookingId = booking.Id }, booking);
        }
    }
}
