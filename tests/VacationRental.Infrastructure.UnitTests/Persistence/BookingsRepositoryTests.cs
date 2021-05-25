using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Infrastructure.Persistence;
using VacationRental.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VacationRental.Infrastructure.UnitTests.Persistence
{
    public class BookingsRepositoryTests
    {
        private readonly BookingsRepository _sut;
        private readonly VacationRentalDbContext _fakeContext;

        public BookingsRepositoryTests()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<VacationRentalDbContext>();
            contextOptionsBuilder.UseInMemoryDatabase("VacationRentalTestDb");
            _fakeContext = new VacationRentalDbContext(contextOptionsBuilder.Options);

            _sut = new BookingsRepository(_fakeContext);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_When_BookingDoesNotEsist()
        {
            // Arrange
            var notExistingBookingId = int.MaxValue;

            // Act
            var booking = await _sut.GetByIdAsync(notExistingBookingId);

            // Assert
            Assert.Null(booking);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnBooking_When_ItExists()
        {
            // Arrange
            var rental = new Rental { Units = 1, PreparationTimeInDays = 1 };
            _fakeContext.Rentals.Add(rental);
            await _fakeContext.SaveChangesAsync();

            var existingBooking = new Booking { RentalId = rental.Id, Start = new DateTime(2001, 01, 10), Nights = 3, Unit = 2 };
            _fakeContext.Bookings.Add(existingBooking);
            await _fakeContext.SaveChangesAsync();

            // Act
            var result = await _sut.GetByIdAsync(existingBooking.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingBooking.Id, result.Id);
            Assert.Equal(existingBooking.RentalId, result.RentalId);
            Assert.Equal(existingBooking.Start, result.Start);
            Assert.Equal(existingBooking.Nights, result.Nights);
            Assert.Equal(existingBooking.Unit, result.Unit);
        }

        [Fact]
        public async Task AddAsync_Should_AddBooking_When_CorrectDataPassed()
        {
            // Arrange
            var rental = new Rental { Units = 1, PreparationTimeInDays = 1 };
            _fakeContext.Rentals.Add(rental);
            await _fakeContext.SaveChangesAsync();

            var booking = new Booking { Id= 0, RentalId = rental.Id, Start = new DateTime(2001, 01, 10), Nights = 3, Unit = 2 };
            
            // Act
            await _sut.AddAsync(booking);

            // Assert
            Assert.NotEqual(0, booking.Id);
        }

        [Fact]
        public async Task GetOverlappingBookings_Should_ReturnEmptyCollection_When_RentalDoesNotHaveBookings()
        {
            // Arrange
            var rentalOne = new Rental { Units = 1, PreparationTimeInDays = 1 };
            var rentalTwo = new Rental { Units = 2, PreparationTimeInDays = 2 };
            _fakeContext.Rentals.AddRange(rentalOne, rentalTwo);
            await _fakeContext.SaveChangesAsync();

            var existingBooking = new Booking { RentalId = rentalOne.Id, Start = new DateTime(2001, 01, 10), Nights = 3, Unit = 2 };
            _fakeContext.Bookings.Add(existingBooking);
            await _fakeContext.SaveChangesAsync();

            // Act
            var result = await _sut.GetOverlappingBookings(rentalTwo.Id, rentalTwo.PreparationTimeInDays, existingBooking.Start, existingBooking.End);

            // Assert
            Assert.Equal(0, result.Count);
        }

        [Theory]
        // Left overlap (end date overlaps with existing boking) 
        [InlineData("01/09/2001", 2, 0, 1)]
        [InlineData("01/09/2001", 1, 1, 1)]
        [InlineData("01/08/2001", 2, 1, 1)]
        [InlineData("01/08/2001", 1, 2, 1)]
        // Right overlap (stat date overlaps with existing booking)
        [InlineData("01/10/2001", 1, 0, 1)]
        [InlineData("01/11/2001", 1, 0, 1)]
        [InlineData("01/10/2001", 1, 1, 1)]
        [InlineData("01/11/2001", 1, 1, 1)]
        // Full overlap (booking range fully overlaps existing booking)
        [InlineData("01/09/2001", 5, 0, 1)]
        [InlineData("01/09/2001", 5, 1, 1)]
        [InlineData("01/09/2001", 5, 5, 1)]
        [InlineData("01/08/2001", 6, 2, 1)]
        // No overlappings
        //[InlineData("01/09/2001", 1, 0, 0)]
        [InlineData("01/08/2001", 1, 1, 0)]
        [InlineData("01/14/2001", 1, 1, 0)]
        public async Task GetOverlappingBookings_Should_ReturnOverlappingBookings_When_Exist(string start, int nights, int preparationTime, int expectedOverlappingBookingsCount)
        {
            // Arrange
            var startDate = Convert.ToDateTime(start);

            var rental = new Rental { Units = 2, PreparationTimeInDays = preparationTime};
            _fakeContext.Rentals.Add(rental);
            await _fakeContext.SaveChangesAsync();

            var booking = new Booking { RentalId = rental.Id, Start = new DateTime(2001, 01, 10), Nights = 3, Unit = 2  };
            _fakeContext.Bookings.Add(booking);
            await _fakeContext.SaveChangesAsync();

            // Act
            var result = await _sut.GetOverlappingBookings(rental.Id, rental.PreparationTimeInDays, startDate, startDate.AddDays(nights));

            // Assert
            Assert.Equal(expectedOverlappingBookingsCount, result.Count);
        }
    }
}
