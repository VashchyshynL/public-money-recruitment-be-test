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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingViewModel> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(query.BookingId);

            return _mapper.Map<BookingViewModel>(booking);
        }
    }
}
