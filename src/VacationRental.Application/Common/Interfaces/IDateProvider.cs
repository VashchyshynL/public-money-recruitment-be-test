using System;

namespace VacationRental.Application.Common.Interfaces
{
    public interface IDateProvider
    {
        DateTime Now { get; }
    }
}
