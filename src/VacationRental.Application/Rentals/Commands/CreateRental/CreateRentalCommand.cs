using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<ResourceIdViewModel>
    {
        public int Units { get; set; }
    }
}
