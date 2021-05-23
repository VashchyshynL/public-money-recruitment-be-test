using FluentValidation;

namespace VacationRental.Application.Rentals.Queries.GetRentalById
{
    public class GetRentalByIdQueryValidator : AbstractValidator<GetRentalByIdQuery>
    {
        public GetRentalByIdQueryValidator()
        {
            RuleFor(r => r.RentalId)
                .GreaterThan(0).WithMessage("RentalId should be greater than 0");
        }
    }
}
