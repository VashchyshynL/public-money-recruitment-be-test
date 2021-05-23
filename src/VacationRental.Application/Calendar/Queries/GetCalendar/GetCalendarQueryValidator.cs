using FluentValidation;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
    {
        public GetCalendarQueryValidator()
        {
            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");
        }
    }
}
