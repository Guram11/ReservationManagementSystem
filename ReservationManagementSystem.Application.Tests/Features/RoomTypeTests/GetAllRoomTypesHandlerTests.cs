using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTypes;

public class GetAllRoomTypesHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllRoomTypesHandler _handler;

    public GetAllRoomTypesHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllRoomTypesHandler(_roomTypeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsRoomTypeResponses()
    {
        // Arrange
        var request = new GetAllRoomTypesRequest("Name", "Deluxe", "Name", true, 1, 10);
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "Deluxe", NumberOfRooms = 5, IsActive = true },
            new RoomType { Id = Guid.NewGuid(), Name = "Standard", NumberOfRooms = 10, IsActive = false }
        };
        var roomTypeResponses = new List<RoomTypeResponse>
        {
            new RoomTypeResponse { Id = roomTypes[0].Id, Name = "Deluxe", NumberOfRooms = 5, IsActive = true },
            new RoomTypeResponse { Id = roomTypes[1].Id, Name = "Standard", NumberOfRooms = 10, IsActive = false }
        };

        _roomTypeRepositoryMock.Setup(r => r.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy, request.IsAscending,
            request.PageNumber, request.PageSize)).ReturnsAsync(roomTypes);

        _mapperMock.Setup(m => m.Map<List<RoomTypeResponse>>(roomTypes)).Returns(roomTypeResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomTypeResponses);
    }

    [Fact]
    public async Task Handle_NoRoomTypes_ReturnsEmptyList()
    {
        // Arrange
        var request = new GetAllRoomTypesRequest(null, null, null, true, 1, 10);
        var roomTypes = new List<RoomType>();
        var roomTypeResponses = new List<RoomTypeResponse>();

        _roomTypeRepositoryMock.Setup(r => r.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy, request.IsAscending,
            request.PageNumber, request.PageSize)).ReturnsAsync(roomTypes);

        _mapperMock.Setup(m => m.Map<List<RoomTypeResponse>>(roomTypes)).Returns(roomTypeResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
    }
}
