using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Common.Models;
using VacationRental.Application.Rentals.Commands.CreateRental;
using VacationRental.Application.Rentals.Commands.UpdateRental;
using VacationRental.Application.Rentals.Queries.GetRentalById;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        [ProducesResponseType(typeof(RentalViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId)
        {
            var rental = await _mediator.Send(new GetRentalByIdQuery {RentalId = rentalId});
            if (rental == null)
                return NotFound("Rental not found");

            return Ok(rental);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResourceIdViewModel>> Post(RentalBindingModel model)
        {
            var rental = await _mediator.Send(new CreateRentalCommand
            {
                Units = model.Units, 
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return CreatedAtAction(nameof(Get), new {rentalId = rental.Id}, rental);
        }

        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, RentalBindingModel model)
        {
            await _mediator.Send(new UpdateRentalCommand
            {
                RentalId = id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return NoContent();
        }
    }
}
