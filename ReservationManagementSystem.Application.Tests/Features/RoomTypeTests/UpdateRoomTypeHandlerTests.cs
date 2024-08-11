using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using FluentAssertions;

namespace ReservationManagementSystem.Application.Tests.Features.RoomTypes;

public class UpdateRoomTypeHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<UpdateRoomTypeRequest>> _validatorMock;
    private readonly UpdateRoomTypeHandler _handler;

    public UpdateRoomTypeHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<UpdateRoomTypeRequest>>();
        _handler = new UpdateRoomTypeHandler(_roomTypeRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid(), "Deluxe", 10, true, 1, 4);
        var roomType = new RoomType { Id = request.Id, Name = request.Name, HotelId = request.HotelId };
        var roomTypeResponse = new RoomTypeResponse { Id = roomType.Id, Name = roomType.Name };

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
        var request = new UpdateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid(), "", 10, true, 1, 4);
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name must be at least 3 characters.")
        };
        var validationResult = new ValidationResult(validationErrors);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Name must be at least 3 characters.");
    }
}
