using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Application.Bookings.Commands.CreateBooking;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Common.Models;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.Application.UnitTests.Bookings
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly CreateBookingCommandHandler _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBookingsRepository> _bookingsRepositoryMock;
        private readonly Mock<IRentalsRepository> _rentalsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateBookingCommandHandlerTests()
        {
            _bookingsRepositoryMock = new Mock<IBookingsRepository>();
            _rentalsRepositoryMock = new Mock<IRentalsRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Bookings).Returns(_bookingsRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Rentals).Returns(_rentalsRepositoryMock.Object);

            _mapperMock = new Mock<IMapper>();
            
            _sut = new CreateBookingCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_When_IncorrectRentalIdPassed()
        {
            var createCommand = new CreateBookingCommand { RentalId = int.MaxValue };

            await Assert.ThrowsAsync<ValidationException>(async () => await _sut.Handle(createCommand, default));
        }

        [Fact]
        public async Task Handle_Should_ThrowConflictException_When_NoAvailableUnitsForBooking()
        {
            var rental = new Rental { Id = 1, Units = 1, PreparationTimeInDays = 1 };
            var existingBooking = new Booking { Id = 1, RentalId = rental.Id, Nights = 2, Start = new DateTime(2001, 01, 01) };

            _rentalsRepositoryMock.Setup(b => b.GetByIdAsync(1)).ReturnsAsync(rental);

            _bookingsRepositoryMock
                .Setup(b => b.GetOverlappingBookings(rental.Id, rental.PreparationTimeInDays, new DateTime(2001, 01, 01), new DateTime(2001, 01, 03)))
                .ReturnsAsync(new[] { existingBooking });


            var createCommand = new CreateBookingCommand { RentalId = 1, Nights = 2, Start = new DateTime(2001, 01, 01) };

            await Assert.ThrowsAsync<ConflictException>(async () => await _sut.Handle(createCommand, default));
        }

        [Theory]
        [InlineData(2, 1, 2)]
        [InlineData(3, 1, 2)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 2, 1)]
        public async Task Handle_Should_CreateBooking_When_AvailableUnitExists(int rentalUnits, int existingBookingUnit, int expectedNewBookingUnit)
        {
            // Arrange
            var rental = new Rental { Id = 1, Units = rentalUnits, PreparationTimeInDays = 1 };
            var existingBooking = new Booking { Id = 1, RentalId = rental.Id, Nights = 2, Start = new DateTime(2001, 01, 01), Unit = existingBookingUnit };

            var startDate = new DateTime(2001, 01, 01);
            var newBooking = new Booking { Id = existingBooking.Id + 1, RentalId = rental.Id, Nights = 1, Start = startDate};

            _mapperMock.Setup(x => x.Map<ResourceIdViewModel>(It.IsAny<Booking>()))
                .Returns((Booking source) => new ResourceIdViewModel { Id = source.Id });

            _rentalsRepositoryMock.Setup(b => b.GetByIdAsync(rental.Id))
                .ReturnsAsync(rental);
                        
            _bookingsRepositoryMock
                .Setup(b => b.GetOverlappingBookings(rental.Id, rental.PreparationTimeInDays, startDate, startDate.AddDays(newBooking.Nights)))
                .ReturnsAsync(new[] { existingBooking });

            var createCommand = new CreateBookingCommand { RentalId = rental.Id, Nights = newBooking.Nights, Start = startDate };

            // Act
            var result = await _sut.Handle(createCommand, default);

            // Assert
            _bookingsRepositoryMock.Verify(b => b.AddAsync(It.Is<Booking>(
                x => x.RentalId == newBooking.RentalId
                && x.Nights == newBooking.Nights
                && x.Start == newBooking.Start
                && x.Unit == expectedNewBookingUnit)), Times.Once());

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            Assert.Equal(0, result.Id);
        }
    }
}
