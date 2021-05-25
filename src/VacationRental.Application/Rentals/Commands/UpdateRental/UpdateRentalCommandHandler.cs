using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VacationRental.Application.Common.Constants;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Rentals.Commands.UpdateRental
{
    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateProvider _dateProvider;

        public UpdateRentalCommandHandler(IUnitOfWork unitOfWork, IDateProvider dateProvider)
        {
            _unitOfWork = unitOfWork;
            _dateProvider = dateProvider;
        }

        public async Task<Unit> Handle(UpdateRentalCommand command, CancellationToken cancellationToken)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(command.RentalId);

            if (rental == null)
                throw new NotFoundException(RentalExceptionMessages.NotFound);

            if (!IsRentalChanged(rental, command)) 
                return Unit.Value;

            var isRentalCanBeUpdated = await IsRentalCanBeUpdated(command);
            if (!isRentalCanBeUpdated)
                throw new ConflictException(RentalExceptionMessages.Conflict);

            rental.Units = command.Units;
            rental.PreparationTimeInDays = command.PreparationTimeInDays;
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }

        private static bool IsRentalChanged(Rental rental, UpdateRentalCommand updateCommand)
        {
            return rental.Units != updateCommand.Units || rental.PreparationTimeInDays != updateCommand.PreparationTimeInDays;
        }

        private async Task<bool> IsRentalCanBeUpdated(UpdateRentalCommand updateCommand)
        {
            var overlappingBookings = 
                await _unitOfWork.Bookings.GetOverlappingBookings(updateCommand.RentalId, updateCommand.PreparationTimeInDays, 
                    _dateProvider.Now, DateTime.MaxValue.AddDays(-updateCommand.PreparationTimeInDays));

            return overlappingBookings.Count <= updateCommand.Units;
        }
    }
}
