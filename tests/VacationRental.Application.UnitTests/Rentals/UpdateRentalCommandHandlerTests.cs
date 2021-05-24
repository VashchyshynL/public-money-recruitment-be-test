using System;
using System.Threading.Tasks;
using MediatR;
using Moq;
using VacationRental.Application.Common.Exceptions;
using VacationRental.Application.Common.Interfaces;
using VacationRental.Application.Rentals.Commands.UpdateRental;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.Application.UnitTests.Rentals
{
    public class UpdateRentalCommandHandlerTests
    {
        private readonly UpdateRentalCommandHandler _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBookingsRepository> _bookingsRepositoryMock;
        private readonly Mock<IRentalsRepository> _rentalsRepositoryMock;

        public UpdateRentalCommandHandlerTests()
        {
            _bookingsRepositoryMock = new Mock<IBookingsRepository>();
            _rentalsRepositoryMock = new Mock<IRentalsRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Bookings).Returns(_bookingsRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Rentals).Returns(_rentalsRepositoryMock.Object);

            _sut = new UpdateRentalCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_When_IncorrectRentalIdPassed()
        {
            var command = CreateCommand(int.MaxValue);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _sut.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_ReturnUnit_When_RentalNotChanged()
        {
            // Arrange
            var rental = CreateRental(1, 1, 1);
            _rentalsRepositoryMock
                .Setup(b => b.GetByIdAsync(rental.Id))
                .ReturnsAsync(rental);

            var command = CreateCommand(rental.Id, rental.Units, rental.PreparationTimeInDays);

            // Act
            var result = await _sut.Handle(command, default);

            // Assert
            Assert.Equal(new Unit(), result);
        }

        [Fact]
        public async Task Handle_Should_ThrowConflictException_When_RentalCanNotBeUpdated()
        {
            // Arrange
            var rental = CreateRental(1, 2, 1);
            var command = CreateCommand(rental.Id, rental.Units - 1, rental.PreparationTimeInDays + 1);

            _rentalsRepositoryMock
                .Setup(b => b.GetByIdAsync(rental.Id))
                .ReturnsAsync(rental);

            _bookingsRepositoryMock
                .Setup(b => b.GetOverlappingBookings(command.RentalId, command.PreparationTimeInDays, 
                    It.IsAny<DateTime>(),It.IsAny<DateTime>()))
                .ReturnsAsync(new[]
                {
                    new Booking { Id = 1, RentalId = rental.Id},
                    new Booking { Id = 2, RentalId = rental.Id},
                    new Booking { Id = 3, RentalId = rental.Id}
                });

            // Act and Assert
            await Assert.ThrowsAsync<ConflictException>(async () => await _sut.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_UpdateRental_When_ValidDataPassed()
        {
            // Arrange
            var rental = CreateRental(1, 1, 1);
            var command = CreateCommand(rental.Id, rental.Units + 1, rental.PreparationTimeInDays);
                
            _rentalsRepositoryMock
                .Setup(b => b.GetByIdAsync(rental.Id))
                .ReturnsAsync(rental);

            _bookingsRepositoryMock
                .Setup(b => b.GetOverlappingBookings(command.RentalId, command.PreparationTimeInDays,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new[] { new Booking { Id = 1, RentalId = rental.Id} });

            // Act
            var result = await _sut.Handle(command, default);

            // Assert
            Assert.Equal(new Unit(), result);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        private static Rental CreateRental(int rentalId, int units, int preparationTime)
        {
            return new Rental {Id = rentalId, Units = units, PreparationTimeInDays = preparationTime};
        }

        private static UpdateRentalCommand CreateCommand(int rentalId, int units = 1, int preparationTime = default)
        {
            return new UpdateRentalCommand
            {
                RentalId = rentalId,
                Units = units,
                PreparationTimeInDays = preparationTime
            };
        }
    }
}
