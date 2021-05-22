using AutoMapper;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Rental, RentalViewModel>();
            CreateMap<Rental, ResourceIdViewModel>();

            CreateMap<Booking, BookingViewModel>();
            CreateMap<Booking, ResourceIdViewModel>();
        }
    }
}
