using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ResourceIdViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResourceIdViewModel> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var rental = await _unitOfWork.Rentals.GetRentalWithBookings(command.RentalId, cancellationToken);

            if (rental == null)
                throw new ValidationException(nameof(command.RentalId), "Rental not found");

            var overlappingBookings = rental.Bookings.Where(b =>
                (b.Start <= command.Start.Date && b.End.AddDays(rental.PreparationTimeInDays) > command.Start.Date)
                || (b.Start < command.End.AddDays(rental.PreparationTimeInDays) && b.End.AddDays(rental.PreparationTimeInDays) >= command.End.AddDays(rental.PreparationTimeInDays))
                || (b.Start > command.Start && b.End.AddDays(rental.PreparationTimeInDays) < command.End.AddDays(rental.PreparationTimeInDays))).ToArray();

            var availableUnit = GetAvailableForBookingUnit(overlappingBookings, rental.Units);

            var booking = new Booking
            {
                RentalId = command.RentalId,
                Unit = availableUnit,
                Start = command.Start, 
                Nights = command.Nights
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ResourceIdViewModel>(booking);
        }


        private static int GetAvailableForBookingUnit(IReadOnlyCollection<Booking> overlappingBookings, int rentalUnits)
        {
            if (rentalUnits > overlappingBookings.Count)
            {
                var bookedRentalUnits = overlappingBookings.Select(booking => booking.Unit).Distinct().ToArray();

                for (var unit = 1; unit <= rentalUnits; unit++)
                {
                    if (!bookedRentalUnits.Contains(unit))
                        return unit;
                }
            }

            throw new ConflictException("Booking not available");
        }
    }
}
