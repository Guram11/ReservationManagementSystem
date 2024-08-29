using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ReservationRooms.Queries;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationRooms;

public class GetAllRateReservationRoomsHandlerTests
{
    private readonly Mock<IReservationRoomRepository> _mockReservationRoomRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllRateReservationRoomsHandler _handler;

    public GetAllRateReservationRoomsHandlerTests()
    {
        _mockReservationRoomRepository = new Mock<IReservationRoomRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllRateReservationRoomsHandler(_mockReservationRoomRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfReservationRoomResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllReservationRoomsRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var reservationRooms = new List<ReservationRoom>
        {
            new ReservationRoom
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
            new ReservationRoom
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationId = Guid.NewGuid(),
                RateId = Guid.NewGuid(),
                RoomTypeId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Checkin = DateTime.UtcNow.AddDays(2),
                Checkout = DateTime.UtcNow.AddDays(4),
                Price = 200.00m
            }
        };

        var reservationRoomResponses = new List<ReservationRoomResponse>
        {
            new ReservationRoomResponse
            {
                Id = reservationRooms[0].Id,
                CreatedAt = reservationRooms[0].CreatedAt,
                UpdatedAt = reservationRooms[0].UpdatedAt,
                ReservationId = reservationRooms[0].ReservationId,
                RateId = reservationRooms[0].RateId,
                RoomTypeId = reservationRooms[0].RoomTypeId,
                RoomId = reservationRooms[0].RoomId,
                Checkin = reservationRooms[0].Checkin,
                Checkout = reservationRooms[0].Checkout,
                Price = reservationRooms[0].Price
            },
            new ReservationRoomResponse
            {
                Id = reservationRooms[1].Id,
                CreatedAt = reservationRooms[1].CreatedAt,
                UpdatedAt = reservationRooms[1].UpdatedAt,
                ReservationId = reservationRooms[1].ReservationId,
                RateId = reservationRooms[1].RateId,
                RoomTypeId = reservationRooms[1].RoomTypeId,
                RoomId = reservationRooms[1].RoomId,
                Checkin = reservationRooms[1].Checkin,
                Checkout = reservationRooms[1].Checkout,
                Price = reservationRooms[1].Price
            }
        };

        _mockReservationRoomRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize, CancellationToken.None
            ))
            .ReturnsAsync(reservationRooms);

        _mockMapper.Setup(m => m.Map<List<ReservationRoomResponse>>(reservationRooms)).Returns(reservationRoomResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationRoomResponses);
    }
}
