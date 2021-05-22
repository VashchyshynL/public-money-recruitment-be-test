using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
    {
        private readonly IVacationRentalDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(IVacationRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var booking = new Booking
            {
                RentalId = command.RentalId, 
                Start = command.Start, 
                Nights = command.Nights
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ResourceIdViewModel>(booking);
        }
    }
}
