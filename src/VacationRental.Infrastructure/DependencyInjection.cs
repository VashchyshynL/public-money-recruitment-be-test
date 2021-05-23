using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Infrastructure.Persistence;

namespace VacationRental.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<VacationRentalDbContext>(options =>
                    options.UseInMemoryDatabase("VacationRentalDb"));
           
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
