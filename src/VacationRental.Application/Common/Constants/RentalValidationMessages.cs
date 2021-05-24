using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Application.Common.Constants
{
    public static class RentalValidationMessages
    {
        public static readonly string PositiveId = "Rental Id should be greater than 0";
        public static readonly string PositiveUnits = "Number of rental units should be greater that 0";
        public static readonly string PositivePreparationTime = "Rental preparation time (in days) should be greater or equal to 0";
    }
}
