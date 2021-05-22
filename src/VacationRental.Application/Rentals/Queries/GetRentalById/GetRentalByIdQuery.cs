using System.Collections.Generic;
using System.Text;
using MediatR;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Rentals.Queries.GetRentalById
{
    public class GetRentalByIdQuery : IRequest<RentalViewModel>
    {
        public int RentalId { get; set; }
    }
}
