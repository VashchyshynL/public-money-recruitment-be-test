using FluentValidation;
using VacationRental.Application.Common.Constants;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
    {
        public GetCalendarQueryValidator()
        {
            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage(BookingValidationMessages.PositiveNights);
        }
    }
}
