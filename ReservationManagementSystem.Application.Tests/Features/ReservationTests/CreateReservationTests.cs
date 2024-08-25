using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

public class CreateReservationHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IAvailibilityTimelineRepository> _availibilityRepositoryMock;
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly Mock<IRateRepository> _rateRepositoryMock;
    private readonly Mock<IRateTimelineRepository> _rateTimelineRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateReservationRequest>> _validatorMock;
    private readonly CreateReservationlHandler _handler;

    public CreateReservationHandlerTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _availibilityRepositoryMock = new Mock<IAvailibilityTimelineRepository>();
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _rateRepositoryMock = new Mock<IRateRepository>();
        _rateTimelineRepositoryMock = new Mock<IRateTimelineRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateReservationRequest>>();
        _handler = new CreateReservationlHandler(_reservationRepositoryMock.Object, _mapperMock.Object,
            _validatorMock.Object, _availibilityRepositoryMock.Object, _roomTypeRepositoryMock.Object,
            _rateTimelineRepositoryMock.Object, _rateRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.UtcNow, DateTime.UtcNow.AddDays(1), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), 2, Currencies.USD);

        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Currency", "Invalid currency type."),
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid currency type.");
    }

    [Fact]
    public async Task Handle_WhenAvailabilityIsNotSufficient_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.UtcNow, DateTime.UtcNow.AddDays(1), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), 3, Currencies.USD);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _availibilityRepositoryMock
            .Setup(repo => repo.GetAvailabilityByDateRange(request.Checkin, request.Checkout))
            .ReturnsAsync(new List<AvailabilityTimeline>
            {
                new AvailabilityTimeline { RoomTypeId = request.RoomTypeId, Available = 2 }
            });

        _roomTypeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true, 1, 10))
            .ReturnsAsync(new List<RoomType> { new RoomType { Id = request.RoomTypeId, Name = "Standard" } });

        _rateTimelineRepositoryMock.Setup(repo => repo.GetRatesByDateRange(request.Checkin, request.Checkout))
            .ReturnsAsync(new List<RateTimeline>
            {
                new RateTimeline { RoomTypeId = request.RoomTypeId, RateId = request.RateId, Price = 100 }
            });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Validation error. Invalid data passed, Please check room availibility");
    }

    [Fact]
    public async Task Handle_WhenNoRatesAvailable_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.UtcNow, DateTime.UtcNow.AddDays(1), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), 2, Currencies.USD);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _availibilityRepositoryMock
            .Setup(repo => repo.GetAvailabilityByDateRange(request.Checkin, request.Checkout))
            .ReturnsAsync(new List<AvailabilityTimeline>
            {
                new AvailabilityTimeline { RoomTypeId = request.RoomTypeId, Available = 2 }
            });

        _roomTypeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true, 1, 10))
            .ReturnsAsync(new List<RoomType> { new RoomType { Id = request.RoomTypeId, Name = "Standard" } });

        _rateTimelineRepositoryMock.Setup(repo => repo.GetRatesByDateRange(request.Checkin, request.Checkout))
            .ReturnsAsync(new List<RateTimeline>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Validation error. Invalid data passed, Please check room availibility");
    }
}