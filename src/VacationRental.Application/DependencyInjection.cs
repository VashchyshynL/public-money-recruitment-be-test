using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Common.Behaviors;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Providers;

namespace VacationRental.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IDateProvider, DateProvider>();

            return services;
        }
    }
}
