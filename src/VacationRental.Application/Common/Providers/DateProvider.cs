using System;
using VacationRental.Application.Common.Interfaces;

namespace VacationRental.Application.Common.Providers
{
    public class DateProvider : IDateProvider
    {
        public DateTime Now => DateTime.Now.Date;
    }
}
