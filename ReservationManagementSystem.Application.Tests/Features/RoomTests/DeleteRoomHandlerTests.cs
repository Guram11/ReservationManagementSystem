using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTests;

public class DeleteRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteRoomHandler _handler;

    public DeleteRoomHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteRoomHandler(_roomRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = new Room { Id = roomId, RoomTypeId = Guid.NewGuid(), Number = "101", Floor = 1, Note = "Nice room" };

        var request = new DeleteRoomRequest(roomId);

        _roomRepositoryMock.Setup(r => r.Delete(roomId, CancellationToken.None))
            .ReturnsAsync(room);

        var roomResponse = new RoomResponse { Id = room.Id, RoomTypeId = room.RoomTypeId, Number = room.Number, Floor = room.Floor, Note = room.Note };

        _mapperMock.Setup(m => m.Map<RoomResponse>(room))
            .Returns(roomResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomResponse);
    }

    [Fact]
    public async Task Handle_RoomNotFound_ReturnsFailureResult()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var request = new DeleteRoomRequest(roomId);

        _roomRepositoryMock.Setup(r => r.Delete(roomId, CancellationToken.None))
            .ReturnsAsync((Room)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain($"Room with ID {roomId} was not found.");
    }
}
