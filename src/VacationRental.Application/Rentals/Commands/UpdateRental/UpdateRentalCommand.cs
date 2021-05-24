using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommand : IRequest
    {
        public int RentalId { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
