using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTests;

public class GetRoomByIdHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetRoomByIdHandler _handler;

    public GetRoomByIdHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetRoomByIdHandler(_roomRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new GetRoomByIdRequest(Guid.NewGuid());
        var room = new Room { Id = request.Id, RoomTypeId = Guid.NewGuid(), Number = "101", Floor = 1, Note = "Nice room" };

        _roomRepositoryMock.Setup(r => r.Get(request.Id))
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
        var request = new GetRoomByIdRequest(Guid.NewGuid());

        _roomRepositoryMock.Setup(r => r.Get(request.Id))
            .ReturnsAsync((Room)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain($"Room with ID {request.Id} was not found.");
    }
}