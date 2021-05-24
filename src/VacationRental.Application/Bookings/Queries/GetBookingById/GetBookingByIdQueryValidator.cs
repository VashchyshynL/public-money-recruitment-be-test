using FluentValidation;
using VacationRental.Application.Common.Constants;

namespace VacationRental.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
    {
        public GetBookingByIdQueryValidator()
        {
            RuleFor(b => b.BookingId)
                .GreaterThan(0).WithMessage(BookingValidationMessages.PositiveId);
        }
    }
}
