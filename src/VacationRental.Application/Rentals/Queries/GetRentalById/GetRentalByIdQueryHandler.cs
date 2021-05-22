using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Rentals.Queries.GetRentalById
{
    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalViewModel>
    {
        private readonly IVacationRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetRentalByIdQueryHandler(IVacationRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RentalViewModel> Handle(GetRentalByIdQuery query, CancellationToken cancellationToken)
        {
            var rental = await _dbContext.Rentals.FindAsync(query.RentalId);

            return _mapper.Map<RentalViewModel>(rental);
        }
    }
}