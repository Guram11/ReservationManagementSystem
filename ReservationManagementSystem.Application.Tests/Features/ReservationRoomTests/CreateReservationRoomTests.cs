using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationRooms;

public class CreateReservationRoomlHandlerTests
{
    private readonly Mock<IReservationRoomRepository> _reservationRoomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateReservationRoomlHandler _handler;

    public CreateReservationRoomlHandlerTests()
    {
        _reservationRoomRepositoryMock = new Mock<IReservationRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateReservationRoomlHandler(
            _reservationRoomRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateReservationRoomRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            150.00m
        );
        var reservationRoom = new ReservationRoom
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationId = request.ReservationId,
            RateId = request.RateId,
            RoomTypeId = request.RoomTypeId,
            RoomId = request.RoomId,
            Checkin = request.Checkin,
            Checkout = request.Checkout,
            Price = request.Price
        };
        var reservationRoomResponse = new ReservationRoomResponse
        {
            Id = reservationRoom.Id,
            CreatedAt = reservationRoom.CreatedAt,
            UpdatedAt = reservationRoom.UpdatedAt,
            ReservationId = reservationRoom.ReservationId,
            RateId = reservationRoom.RateId,
            RoomTypeId = reservationRoom.RoomTypeId,
            RoomId = reservationRoom.RoomId,
            Checkin = reservationRoom.Checkin,
            Checkout = reservationRoom.Checkout,
            Price = reservationRoom.Price
        };

        _mapperMock.Setup(m => m.Map<ReservationRoom>(request)).Returns(reservationRoom);
        _mapperMock.Setup(m => m.Map<ReservationRoomResponse>(reservationRoom)).Returns(reservationRoomResponse);
        _reservationRoomRepositoryMock.Setup(repo => repo.Create(reservationRoom)).ReturnsAsync(reservationRoom);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationRoomResponse);
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsNull_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationRoomRequest(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            150.00m
        );

        _mapperMock.Setup(m => m.Map<ReservationRoom>(request)).Returns(new ReservationRoom());
        _reservationRoomRepositoryMock.Setup(repo => repo.Create(It.IsAny<ReservationRoom>())).ReturnsAsync((ReservationRoom)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("ReservationRoom not found");
    }
}
