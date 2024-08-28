using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateRoomTypeTests;

public class DeleteRateRoomTypeHandlerTests
{
    private readonly Mock<IRateRoomTypeRepository> _rateRoomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteRateRoomTypeHandler _handler;

    public DeleteRateRoomTypeHandlerTests()
    {
        _rateRoomTypeRepositoryMock = new Mock<IRateRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteRateRoomTypeHandler(_rateRoomTypeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RateRoomTypeNotFound_ReturnsFailureResult()
    {
        // Arrange
        var request = new DeleteRateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid());
        _rateRoomTypeRepositoryMock.Setup(x => x.Delete(request.RateId, request.RoomTypeId))
            .ReturnsAsync((RateRoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("NotFound");
        result.Error.Description.Should().Be("RateRoomType was not found.");
    }

    [Fact]
    public async Task Handle_RateRoomTypeFound_ReturnsSuccessResult()
    {
        // Arrange
        var rateRoomType = new RateRoomType
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RateId = Guid.NewGuid(),
            RoomTypeId = Guid.NewGuid()
        };

        var request = new DeleteRateRoomTypeRequest(rateRoomType.RateId, rateRoomType.RoomTypeId);

        _rateRoomTypeRepositoryMock.Setup(x => x.Delete(request.RateId, request.RoomTypeId))
            .ReturnsAsync(rateRoomType);

        var rateRoomTypeResponse = new RateRoomTypeResponse
        {
            CreatedAt = rateRoomType.CreatedAt,
            UpdatedAt = rateRoomType.UpdatedAt,
            RateId = rateRoomType.RateId,
            RoomTypeId = rateRoomType.RoomTypeId
        };

        _mapperMock.Setup(x => x.Map<RateRoomTypeResponse>(rateRoomType))
            .Returns(rateRoomTypeResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(rateRoomTypeResponse);
    }
}
