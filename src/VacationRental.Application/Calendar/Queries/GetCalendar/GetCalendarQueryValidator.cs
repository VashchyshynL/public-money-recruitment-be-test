using FluentValidation;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Calendar.Queries.GetCalendar
{
    public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
    {
        public GetCalendarQueryValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(b => b.Nights)
                .GreaterThan(0).WithMessage("Nights must be positive");

            RuleFor(r => r.RentalId)
                .MustAsync(unitOfWork.Rentals.IsExists).WithMessage("Rental not found");
        }
    }
}
