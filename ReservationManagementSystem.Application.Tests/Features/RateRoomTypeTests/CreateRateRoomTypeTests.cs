using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateRoomTypeTests;

public class CreateRateRoomTypeHandlerTests
{
    private readonly Mock<IRateRoomTypeRepository> _mockRateRoomTypeRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateRateRoomTypeHandler _handler;

    public CreateRateRoomTypeHandlerTests()
    {
        _mockRateRoomTypeRepository = new Mock<IRateRoomTypeRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateRateRoomTypeHandler(_mockRateRoomTypeRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRateRoomTypeIsNotCreated()
    {
        // Arrange
        var request = new CreateRateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid());
        var rateRoomType = new RateRoomType();

        _mockMapper.Setup(m => m.Map<RateRoomType>(request)).Returns(rateRoomType);
        _mockRateRoomTypeRepository.Setup(r => r.Create(rateRoomType)).ReturnsAsync((RateRoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Description.Should().Contain("RateRoomType was not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenRateRoomTypeIsCreated()
    {
        // Arrange
        var request = new CreateRateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid());
        var rateRoomType = new RateRoomType
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RateId = request.RateId,
            RoomTypeId = request.RoomTypeId
        };
        var createdRateRoomType = new RateRoomType
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RateId = request.RateId,
            RoomTypeId = request.RoomTypeId
        };
        var response = new RateRoomTypeResponse
        {
            CreatedAt = createdRateRoomType.CreatedAt,
            UpdatedAt = createdRateRoomType.UpdatedAt,
            RateId = createdRateRoomType.RateId,
            RoomTypeId = createdRateRoomType.RoomTypeId
        };

        // Setup the mapper to correctly map the domain model to the response model
        _mockMapper.Setup(m => m.Map<RateRoomType>(request)).Returns(rateRoomType);
        _mockRateRoomTypeRepository.Setup(r => r.Create(rateRoomType)).ReturnsAsync(createdRateRoomType);
        _mockMapper.Setup(m => m.Map<RateRoomTypeResponse>(createdRateRoomType)).Returns(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}