using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.RateTimelines.Common;
using ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateTimelineTests;

public class PushPriceHandlerTests
{
    private readonly Mock<IRateTimelineRepository> _rateTimelineRepositoryMock;
    private readonly Mock<IRateRoomTypeRepository> _rateRoomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PushPriceHandler _handler;

    public PushPriceHandlerTests()
    {
        _rateTimelineRepositoryMock = new Mock<IRateTimelineRepository>();
        _rateRoomTypeRepositoryMock = new Mock<IRateRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new PushPriceHandler(
            _rateTimelineRepositoryMock.Object,
            _rateRoomTypeRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesOrCreatesRateTimeline()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(2);
        var price = 100m;
        var request = new PushPriceRequest(rateId, roomTypeId, startDate, endDate, price);

        var rateRoomType = new RateRoomType
        {
            RateId = rateId,
            RoomTypeId = roomTypeId,
            RateTimelines = new List<RateTimeline>()
        };

        var existingRateTimeline = new RateTimeline
        {
            Date = startDate,
            RateId = rateId,
            RoomTypeId = roomTypeId,
            Price = 90m
        };

        _rateRoomTypeRepositoryMock.Setup(r => r.GetRateRoomTypeWithRateTimelines(rateId, roomTypeId))
            .ReturnsAsync(rateRoomType);

        _rateRoomTypeRepositoryMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<RateTimelineResponse>(It.IsAny<RateTimeline>()))
            .Returns(new RateTimelineResponse());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _rateTimelineRepositoryMock.Verify(r => r.Create(It.IsAny<RateTimeline>()), Times.Exactly(3));
        _rateRoomTypeRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RateRoomTypeNotFound_ReturnsFailure()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid();
        var request = new PushPriceRequest(rateId, roomTypeId, DateTime.Now, DateTime.Now.AddDays(1), 100m);

        _rateRoomTypeRepositoryMock.Setup(r => r.GetRateRoomTypeWithRateTimelines(rateId, roomTypeId))
            .ReturnsAsync((RateRoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be("RateRoomType was not found.");
    }
}
