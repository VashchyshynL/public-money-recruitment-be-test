using System;

namespace VacationRental.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; set; }
        public int Nights { get; set; }
        public DateTime Start { get; set; }
        public DateTime End => Start.AddDays(Nights).Date;
    }
}
