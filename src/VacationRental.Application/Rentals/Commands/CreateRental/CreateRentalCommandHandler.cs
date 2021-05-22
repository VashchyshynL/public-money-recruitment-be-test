using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, ResourceIdViewModel>
    {
        private readonly IVacationRentalDbContext _context;
        private readonly IMapper _mapper;

        public CreateRentalCommandHandler(IVacationRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var rental = new Rental {Units = command.Units};
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResourceIdViewModel>(rental);
        }
    }
}
