using System.Collections.Generic;
using System.Text;
using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Rentals.Queries.GetRentals
{
    public class GetRentalByIdQuery : IRequest<RentalViewModel>
    {
        public int RentalId { get; set; }
    }
}
