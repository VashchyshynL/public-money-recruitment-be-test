using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using VacationRental.Application.Common.Interfaces;
using ValidationException = VacationRental.Application.Common.Exceptions.ValidationException;

namespace VacationRental.Application.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(b => b.RentalId)
                .GreaterThan(0).WithMessage("RentalId should be greater than 0");

            RuleFor(b => b)
                .MustAsync(IsBookingAvailable).WithMessage("Booking not available");
        }

        private async Task<bool> IsBookingAvailable(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var rental = await _unitOfWork.Rentals.GetRentalWithBookings(command.RentalId, cancellationToken);

            if (rental == null)
                throw new ValidationException(nameof(command.RentalId), "Rental not found");
           
            var count = rental.Bookings.Count(b => (b.Start <= command.Start.Date && b.Start.AddDays(b.Nights) > command.Start.Date) 
                                                    || (b.Start < command.Start.AddDays(command.Nights) && b.Start.AddDays(b.Nights) >= command.Start.AddDays(command.Nights)) 
                                                    || (b.Start > command.Start && b.Start.AddDays(b.Nights) < command.Start.AddDays(command.Nights)));

            return rental.Units > count;
           
        }
    }
}
