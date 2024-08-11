using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using FluentAssertions;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTests;

public class UpdateRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<UpdateRoomRequest>> _validatorMock;
    private readonly UpdateRoomHandler _handler;

    public UpdateRoomHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<UpdateRoomRequest>>();
        _handler = new UpdateRoomHandler(_roomRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateRoomRequest(Guid.NewGuid(), Guid.NewGuid(), "101", 1, "Nice room");
        var room = new Room { Id = request.Id, RoomTypeId = request.RoomTypeId, Number = request.Number, Floor = request.Floor, Note = request.Note };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(m => m.Map<Room>(request))
            .Returns(room);

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
    public async Task Handle_InvalidRequest_ReturnsFailureResult()
    {
        // Arrange
        var request = new UpdateRoomRequest(Guid.NewGuid(), Guid.NewGuid(), "", 0, null);
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Number", "Number must be at least 3 characters."),
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Number must be at least 3 characters.");
    }
}
