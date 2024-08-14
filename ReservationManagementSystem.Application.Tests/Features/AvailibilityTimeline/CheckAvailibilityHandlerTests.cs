using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.AvailibilityTimeline;

public class CheckAvailibilityHandlerTests
{
    public class CheckAvailabilityHandlerTests
    {
        private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
        private readonly Mock<IAvailibilityTimelineRepository> _availabilityTimelineRepositoryMock;
        private readonly Mock<IRateTimelineRepository> _rateTimelineRepositoryMock;
        private readonly Mock<IRateRepository> _rateRepositoryMock;
        private readonly CheckAvailabilityHandler _handler;

        public CheckAvailabilityHandlerTests()
        {
            _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
            _availabilityTimelineRepositoryMock = new Mock<IAvailibilityTimelineRepository>();
            _rateTimelineRepositoryMock = new Mock<IRateTimelineRepository>();
            _rateRepositoryMock = new Mock<IRateRepository>();

            _handler = new CheckAvailabilityHandler(
                _roomTypeRepositoryMock.Object,
                _availabilityTimelineRepositoryMock.Object,
                _rateTimelineRepositoryMock.Object,
                _rateRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenDataIsComplete()
        {
            // Arrange
            var roomTypeId = Guid.NewGuid();
            var rateId = Guid.NewGuid();
            var request = new CheckAvailabilityRequest(DateTime.Now, DateTime.Now.AddDays(2));
            var roomTypes = new List<RoomType>
            {
                new RoomType { Id = roomTypeId, Name = "Deluxe" }
            };

            var availabilityTimelines = new List<AvailabilityTimeline>
            {
                new AvailabilityTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom, Available = 5 },
                new AvailabilityTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(1), Available = 5 },
                new AvailabilityTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(2), Available = 5 }
            };

            var rateTimelines = new List<RateTimeline>
            {
                new RateTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom, Price = 100, RateId = rateId },
                new RateTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(1), Price = 100, RateId = rateId },
                new RateTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(2), Price = 100, RateId = rateId }
            };

            var rate = new Rate { Id = rateId, Name = "Standard" };

            _roomTypeRepositoryMock.Setup(repo => repo.GetAll(null, null, null, true, 1, 10))
                .ReturnsAsync(roomTypes);

            _availabilityTimelineRepositoryMock.Setup(repo => repo.GetAvailabilityByDateRange(request.DateFrom.Date, request.DateTo.Date))
                .ReturnsAsync(availabilityTimelines);

            _rateTimelineRepositoryMock.Setup(repo => repo.GetRatesByDateRange(request.DateFrom.Date, request.DateTo.Date))
                .ReturnsAsync(rateTimelines);

            _rateRepositoryMock.Setup(repo => repo.Get(rateId))
                .ReturnsAsync(rate);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.First().RoomType.Should().Be("Deluxe-Standard");
            result.Data.First().AvailableRooms.Should().Be(5);
            result.Data.First().TotalPrice.Should().Be(300);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenDataIsMissing()
        {
            // Arrange
            var roomTypeId = Guid.NewGuid();
            var request = new CheckAvailabilityRequest(DateTime.Now, DateTime.Now.AddDays(2));

            var roomTypes = new List<RoomType>
            {
                new RoomType { Id = roomTypeId, Name = "Deluxe" }
            };

            var availabilityTimelines = new List<AvailabilityTimeline>
            {
                // Note that one day is missing
                new AvailabilityTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom, Available = 5 },
                new AvailabilityTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(1), Available = 5 }
            };

            var rateTimelines = new List<RateTimeline>
            {
                // Note that one day is missing
                new RateTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom, Price = 100, RateId = Guid.NewGuid() },
                new RateTimeline { RoomTypeId = roomTypeId, Date = request.DateFrom.AddDays(1), Price = 100, RateId = Guid.NewGuid() }
            };

            _roomTypeRepositoryMock.Setup(repo => repo.GetAll(null, null, null, true, 1, 10))
                .ReturnsAsync(roomTypes);

            _availabilityTimelineRepositoryMock.Setup(repo => repo.GetAvailabilityByDateRange(request.DateFrom.Date, request.DateTo.Date))
                .ReturnsAsync(availabilityTimelines);

            _rateTimelineRepositoryMock.Setup(repo => repo.GetRatesByDateRange(request.DateFrom.Date, request.DateTo.Date))
                .ReturnsAsync(rateTimelines);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Description.Should().Contain("Availability or rate data missing");
        }
    }
}
