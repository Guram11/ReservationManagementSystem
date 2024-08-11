using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTypes;

public class DeleteRoomTypeHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteRoomTypeHandler _handler;

    public DeleteRoomTypeHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteRoomTypeHandler(_roomTypeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new DeleteRoomTypeRequest(Guid.NewGuid());
        var roomType = new RoomType { Id = request.Id, Name = "Deluxe" };
        var roomTypeResponse = new RoomTypeResponse { Id = roomType.Id, Name = roomType.Name };

        _roomTypeRepositoryMock.Setup(r => r.Delete(request.Id)).ReturnsAsync(roomType);
        _mapperMock.Setup(m => m.Map<RoomTypeResponse>(roomType)).Returns(roomTypeResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Handle_RoomTypeNotFound_ReturnsFailureResult()
    {
        // Arrange
        var request = new DeleteRoomTypeRequest(Guid.NewGuid());

        _roomTypeRepositoryMock.Setup(r => r.Delete(request.Id)).ReturnsAsync((RoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain($"RoomType with ID {request.Id} was not found.");
    }
}
