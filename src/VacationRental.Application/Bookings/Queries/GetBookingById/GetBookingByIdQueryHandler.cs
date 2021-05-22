using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;

namespace VacationRental.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingViewModel>
    {
        private readonly IVacationRentalDbContext _context;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IVacationRentalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingViewModel> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.FindAsync(query.BookingId);
            
            return _mapper.Map<BookingViewModel>(booking);
        }
    }
}
