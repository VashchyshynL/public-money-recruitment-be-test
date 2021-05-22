using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Application.Common.Models;
using VacationRental.Application.Rentals.Commands.CreateRental;
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
        public async Task<RentalViewModel> Get(int rentalId)
        {
            return await _mediator.Send(new GetRentalByIdQuery {RentalId = rentalId});
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            return await _mediator.Send(new CreateRentalCommand
            {
                Units = model.Units, 
                PreparationTimeInDays = model.PreparationTimeInDays
            });
        }
    }
}
