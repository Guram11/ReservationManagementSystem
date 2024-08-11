using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using FluentAssertions;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTypes;

public class CreateRoomTypeHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateRoomTypeRequest>> _validatorMock;
    private readonly CreateRoomTypeHandler _handler;

    public CreateRoomTypeHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateRoomTypeRequest>>();
        _handler = new CreateRoomTypeHandler(_roomTypeRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateRoomTypeRequest(Guid.NewGuid(), "Deluxe", 10, true, 1, 2);
        var roomType = new RoomType { Id = Guid.NewGuid(), HotelId = request.HotelId, Name = request.Name, NumberOfRooms = request.NumberOfRooms, IsActive = request.IsActive, MinCapacity = request.MinCapacity, MaxCapacity = request.MaxCapacity };
        var roomTypeResponse = new RoomTypeResponse { Id = roomType.Id, HotelId = roomType.HotelId, Name = roomType.Name, NumberOfRooms = roomType.NumberOfRooms, IsActive = roomType.IsActive, MinCapacity = roomType.MinCapacity, MaxCapacity = roomType.MaxCapacity };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(m => m.Map<RoomType>(request)).Returns(roomType);
        _mapperMock.Setup(m => m.Map<RoomTypeResponse>(roomType)).Returns(roomTypeResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateRoomTypeRequest(Guid.NewGuid(), "", 10, true, 1, 2);
        var validationFailures = new ValidationFailure[]
        {
            new ValidationFailure("Name", "Name must be at least 3 characters.")
        };
        var validationResult = new ValidationResult(validationFailures);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Name must be at least 3 characters.");
    }
}
