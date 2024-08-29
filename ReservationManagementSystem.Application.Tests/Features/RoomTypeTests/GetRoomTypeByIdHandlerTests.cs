using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTypes;

public class GetRoomTypeByIdHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetRoomTypeByIdHandler _handler;

    public GetRoomTypeByIdHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetRoomTypeByIdHandler(_roomTypeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsRoomTypeResponse()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var request = new GetRoomTypeByIdRequest(roomId);
        var roomType = new RoomType { Id = roomId, Name = "Deluxe", NumberOfRooms = 5, IsActive = true };
        var roomTypeResponse = new RoomTypeResponse { Id = roomType.Id, Name = "Deluxe", NumberOfRooms = 5, IsActive = true };

        _roomTypeRepositoryMock.Setup(r => r.GetRoomTypeWithAvailabilityAsync(roomId, CancellationToken.None))
            .ReturnsAsync(roomType);
        _mapperMock.Setup(m => m.Map<RoomTypeResponse>(roomType))
            .Returns(roomTypeResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Handle_RoomTypeNotFound_ReturnsFailure()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var request = new GetRoomTypeByIdRequest(roomId);

        _roomTypeRepositoryMock.Setup(r => r.GetRoomTypeWithAvailabilityAsync(roomId, CancellationToken.None))
            .ReturnsAsync((RoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be($"RoomType with ID {roomId} was not found.");
    }
}
