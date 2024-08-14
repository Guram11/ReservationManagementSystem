using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.AvailibilityTimeline;

public class PushAvailabilityHandlerTests
{
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IAvailibilityTimelineRepository> _availabilityTimelineRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PushAvailabilityHandler _handler;

    public PushAvailabilityHandlerTests()
    {
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _availabilityTimelineRepositoryMock = new Mock<IAvailibilityTimelineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new PushAvailabilityHandler(
            _roomTypeRepositoryMock.Object,
            _availabilityTimelineRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesAvailability()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(2);
        var availableRooms = (byte)5;
        var request = new PushAvailabilityRequest(roomTypeId, startDate, endDate, availableRooms);

        var existingRoomType = new RoomType
        {
            Id = roomTypeId,
            NumberOfRooms = 10,
            Name = "RoomType1",
            AvailabilityTimelines = new List<AvailabilityTimeline>()
        };

        var availabilityTimeline = new AvailabilityTimeline
        {
            Id = Guid.NewGuid(),
            Date = startDate,
            RoomTypeId = roomTypeId,
            Available = availableRooms
        };

        _roomTypeRepositoryMock.Setup(r => r.GetRoomTypeWithAvailabilityAsync(roomTypeId))
            .ReturnsAsync(existingRoomType);

        _roomTypeRepositoryMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<AvailabilityResponse>(It.IsAny<AvailabilityTimeline>()))
            .Returns(new AvailabilityResponse
            {
                Id = availabilityTimeline.Id,
                Date = availabilityTimeline.Date,
                RoomTypeId = availabilityTimeline.RoomTypeId,
                Available = availabilityTimeline.Available
            });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(new AvailabilityResponse
        {
            Id = availabilityTimeline.Id,
            Date = startDate,
            RoomTypeId = roomTypeId,
            Available = availableRooms
        });

        _availabilityTimelineRepositoryMock.Verify(a => a.Create(It.IsAny<AvailabilityTimeline>()), Times.Exactly(3));
        _roomTypeRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RoomTypeNotFound_ReturnsFailure()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var request = new PushAvailabilityRequest(roomTypeId, DateTime.Now, DateTime.Now.AddDays(1), 5);

        _roomTypeRepositoryMock.Setup(r => r.GetRoomTypeWithAvailabilityAsync(roomTypeId))
            .ReturnsAsync((RoomType)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be("Room type not found.");
    }

    [Fact]
    public async Task Handle_AvailableRoomsExceedTotalNumber_ReturnsFailure()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var request = new PushAvailabilityRequest(roomTypeId, DateTime.Now, DateTime.Now.AddDays(1), 15);

        var existingRoomType = new RoomType
        {
            Id = roomTypeId,
            NumberOfRooms = 10,
            Name = "RoomType1",
            AvailabilityTimelines = new List<AvailabilityTimeline>()
        };

        _roomTypeRepositoryMock.Setup(r => r.GetRoomTypeWithAvailabilityAsync(roomTypeId))
            .ReturnsAsync(existingRoomType);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be("Requested available rooms exceed the total number of rooms (10).");
    }
}
