using AutoMapper;
using Moq;
using System;
using System.Threading.Tasks;
using VacationRental.Application.Bookings.Queries.GetBookingById;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.Application.UnitTests.Bookings
{
    public class GetBookingByIdQueryHandlerTests
    {
        private readonly GetBookingByIdQueryHandler _sut;
        private readonly Mock<IBookingsRepository> _bookingsRepositoryMock;

        public GetBookingByIdQueryHandlerTests()
        {
            _bookingsRepositoryMock = new Mock<IBookingsRepository>();
            
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Bookings).Returns(_bookingsRepositoryMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<BookingViewModel>(It.IsAny<Booking>()))
                .Returns((Booking source) =>
                {
                    if (source == null)
                        return null;

                    return new BookingViewModel
                    {
                        Id = source.Id,
                        RentalId = source.RentalId,
                        Nights = source.Nights,
                        Start = source.Start,
                    };
                 });

            _sut = new GetBookingByIdQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnBooking_When_CorrectBookingIdPassed()
        {
            // Arrange
            var existingBooking = new Booking { Id = 1, RentalId = 1, Nights = 3, Start = new DateTime(2001, 01, 01) };

            _bookingsRepositoryMock.Setup(b => b.GetByIdAsync(1))
                .ReturnsAsync(existingBooking);

            // Act
            var result = await _sut.Handle(new GetBookingByIdQuery { BookingId = 1 }, default);

            // Assert
            Assert.Equal(existingBooking.Id, result.Id);
            Assert.Equal(existingBooking.RentalId, result.RentalId);
            Assert.Equal(existingBooking.Nights, result.Nights);
            Assert.Equal(existingBooking.Start, result.Start);
        }

        [Fact]
        public async Task Handle_Should_ReturnNull_When_IncorrectBookingIdPassed()
        {
            // Arrange
            var getBookingQuery = new GetBookingByIdQuery { BookingId = int.MaxValue };

            // Act
            var result = await _sut.Handle(getBookingQuery, default);

            // Assert
            Assert.Null(result);
        }
    }
}
