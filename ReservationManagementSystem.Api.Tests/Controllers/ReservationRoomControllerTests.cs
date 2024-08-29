using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.DeleteReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ReservationRooms.Queries;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class ReservationRoomsControllerTests
{
    private readonly ReservationRoomsController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public ReservationRoomsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReservationRoomsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfReservationRoomResponse()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "RoomId",
            FilterQuery = "some-room-id",
            SortBy = "Price",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };
        var reservations = new List<ReservationRoomResponse>
        {
            new ReservationRoomResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationId = Guid.NewGuid(),
                RateId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Checkin = DateTime.UtcNow.AddDays(1),
                Checkout = DateTime.UtcNow.AddDays(3),
                Price = 150.00m
            },
            new ReservationRoomResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationId = Guid.NewGuid(),
                RateId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Checkin = DateTime.UtcNow.AddDays(4),
                Checkout = DateTime.UtcNow.AddDays(6),
                Price = 200.00m
            }
        };
        var result = Result<List<ReservationRoomResponse>>.Success(reservations);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationRoomsRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(reservations);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithReservationRoomResponse()
    {
        // Arrange
        var request = new CreateReservationRoomRequest(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), 150.00m);
        var reservation = new ReservationRoomResponse
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
        var result = Result<ReservationRoomResponse>.Success(reservation);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(reservation);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithReservationRoomResponse()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var reservation = new ReservationRoomResponse
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
        var result = Result<ReservationRoomResponse>.Success(reservation);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationRoomRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(reservationId, roomId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(reservation);
    }
}
