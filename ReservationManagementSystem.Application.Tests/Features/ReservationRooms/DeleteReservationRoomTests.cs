using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.DeleteReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationRooms;

public class DeleteReservationRoomHandlerTests
{
    private readonly Mock<IReservationRoomRepository> _mockReservationRoomRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteReservationRoomHandler _handler;

    public DeleteReservationRoomHandlerTests()
    {
        _mockReservationRoomRepository = new Mock<IReservationRoomRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteReservationRoomHandler(_mockReservationRoomRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenReservationRoomIsDeleted()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var reservationRoom = new ReservationRoom
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationId = reservationId,
            RateId = Guid.NewGuid(),
            RoomTypeId = Guid.NewGuid(),
            RoomId = roomId,
            Checkin = DateTime.UtcNow.AddDays(1),
            Checkout = DateTime.UtcNow.AddDays(3),
            Price = 150.00m
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

        _mockReservationRoomRepository.Setup(repo => repo.Delete(reservationId, roomId)).ReturnsAsync(reservationRoom);
        _mockMapper.Setup(m => m.Map<ReservationRoomResponse>(reservationRoom)).Returns(reservationRoomResponse);

        var request = new DeleteReservationRoomRequest(reservationId, roomId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationRoomResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReservationRoomNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        _mockReservationRoomRepository.Setup(repo => repo.Delete(reservationId, roomId)).ReturnsAsync((ReservationRoom)null!);

        var request = new DeleteReservationRoomRequest(reservationId, roomId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("NotFound");
        result.Error.Description.Should().Be("ReservationRoom was not found!");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        _mockReservationRoomRepository.Setup(repo => repo.Delete(reservationId, roomId)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteReservationRoomRequest(reservationId, roomId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
