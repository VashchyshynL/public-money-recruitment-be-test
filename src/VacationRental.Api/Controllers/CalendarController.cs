using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Calendar.Queries.GetCalendar;
using VacationRental.Application.Common.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            return await _mediator.Send(new GetCalendarQuery
            {
                RentalId = rentalId, 
                Start = start, 
                Nights = nights
            });
        }
    }
}
