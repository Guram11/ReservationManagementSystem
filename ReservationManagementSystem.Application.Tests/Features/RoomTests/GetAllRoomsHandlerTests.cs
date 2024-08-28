using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTests;

public class GetAllRoomsHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllRoomsHandler _handler;

    public GetAllRoomsHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllRoomsHandler(_roomRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsListOfRoomResponses()
    {
        // Arrange
        var request = new GetAllRoomsRequest(null, null, null, true, 1, 10);
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), RoomTypeId = Guid.NewGuid(), Number = "101", Floor = 1, Note = "Nice room" },
            new Room { Id = Guid.NewGuid(), RoomTypeId = Guid.NewGuid(), Number = "102", Floor = 2, Note = "Sea view" }
        };

        _roomRepositoryMock.Setup(r => r.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize))
            .ReturnsAsync(rooms);

        var roomResponses = new List<RoomResponse>
        {
            new RoomResponse { Id = rooms[0].Id, RoomTypeId = rooms[0].RoomTypeId, Number = rooms[0].Number, Floor = rooms[0].Floor, Note = rooms[0].Note },
            new RoomResponse { Id = rooms[1].Id, RoomTypeId = rooms[1].RoomTypeId, Number = rooms[1].Number, Floor = rooms[1].Floor, Note = rooms[1].Note }
        };

        _mapperMock.Setup(m => m.Map<List<RoomResponse>>(rooms))
            .Returns(roomResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomResponses);
    }
}
