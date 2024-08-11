using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using FluentAssertions;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTests;

public class CreateRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateRoomRequest>> _validatorMock;
    private readonly CreateRoomHandler _handler;

    public CreateRoomHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateRoomRequest>>();
        _handler = new CreateRoomHandler(_roomRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateRoomRequest(Guid.NewGuid(), "101", 1, "Nice room");
        var room = new Room { Id = Guid.NewGuid(), RoomTypeId = request.RoomTypeId, Number = request.Number, Floor = request.Floor, Note = request.Note };

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
        var request = new CreateRoomRequest(Guid.NewGuid(), "", 0, null);
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
