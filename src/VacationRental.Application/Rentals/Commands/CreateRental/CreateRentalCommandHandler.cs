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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRentalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateRentalCommand command, CancellationToken cancellationToken)
        {
            var rental = new Rental
            {
                Units = command.Units,
                PreparationTimeInDays = command.PreparationTimeInDays
            };

            await _unitOfWork.Rentals.AddAsync(rental);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ResourceIdViewModel>(rental);
        }
    }
}
