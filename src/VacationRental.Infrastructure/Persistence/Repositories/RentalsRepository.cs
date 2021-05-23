using VacationRental.Application.Common.Interfaces;
using VacationRental.Domain.Entities;

namespace VacationRental.Infrastructure.Persistence.Repositories
{
    public class RentalsRepository : Repository<Rental>, IRentalsRepository
    {
        public RentalsRepository(VacationRentalDbContext context) : base(context)
        {
        }
    }
}
