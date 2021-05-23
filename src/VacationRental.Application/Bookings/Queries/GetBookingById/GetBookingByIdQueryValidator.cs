using FluentValidation;

namespace VacationRental.Application.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
    {
        public GetBookingByIdQueryValidator()
        {
            RuleFor(b => b.BookingId)
                .GreaterThan(0).WithMessage("BookingId should be greater than 0");
        }
    }
}
