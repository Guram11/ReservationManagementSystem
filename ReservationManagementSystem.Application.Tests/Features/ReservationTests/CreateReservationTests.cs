using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

public class CreateReservationHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateReservationRequest>> _validatorMock;
    private readonly Mock<IRequestHandler<CheckAvailabilityRequest, Result<List<CheckAvailabilityResponse>>>> _checkAvailabilityHandlerMock;
    private readonly CreateReservationHandler _handler;

    public CreateReservationHandlerTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateReservationRequest>>();
        _checkAvailabilityHandlerMock = new Mock<IRequestHandler<CheckAvailabilityRequest, Result<List<CheckAvailabilityResponse>>>>();
        _handler = new CreateReservationHandler(
            _reservationRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _checkAvailabilityHandlerMock.Object
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenValidationFails()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, Currencies.USD);
        var validationFailures = new List<ValidationFailure> { new ValidationFailure("Property", "Error") };
        var validationResult = new ValidationResult(validationFailures);

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Error");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenAvailabilityCheckFails()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, Currencies.USD);
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _checkAvailabilityHandlerMock.Setup(c => c.Handle(It.IsAny<CheckAvailabilityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<CheckAvailabilityResponse>>.Failure(new Error(ErrorType.InvalidDataPassedError, "Availability check failed")));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid data passed, Please check room availability");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenNoValidResponseIsFound()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, Currencies.USD);
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var availabilityResponses = new List<CheckAvailabilityResponse>
        {
            new CheckAvailabilityResponse { RoomTypeId = Guid.NewGuid(), RoomType = "RoomType", RateId = Guid.NewGuid(), AvailableRooms = 0 }
        };
        _checkAvailabilityHandlerMock.Setup(c => c.Handle(It.IsAny<CheckAvailabilityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<CheckAvailabilityResponse>>.Success(availabilityResponses));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid data passed, Please check room availability");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenReservationIsCreatedSuccessfully()
    {
        // Arrange
        var request = new CreateReservationRequest(DateTime.Now, DateTime.Now.AddDays(1), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, Currencies.USD);
        var validationResult = new ValidationResult();
        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var validResponse = new CheckAvailabilityResponse
        {
            RoomTypeId = request.RoomTypeId,
            RoomType = "RoomType",
            RateId = request.RateId,
            AvailableRooms = 2
        };
        _checkAvailabilityHandlerMock.Setup(c => c.Handle(It.IsAny<CheckAvailabilityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<CheckAvailabilityResponse>>.Success(new List<CheckAvailabilityResponse> { validResponse }));

        var reservation = new Reservation();
        _mapperMock.Setup(m => m.Map<Reservation>(request)).Returns(reservation);
        _reservationRepositoryMock.Setup(r => r.Create(reservation)).ReturnsAsync(reservation);

        var reservationResponse = new ReservationResponse();
        _mapperMock.Setup(m => m.Map<ReservationResponse>(reservation)).Returns(reservationResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationResponse, options => options.ComparingByMembers<ReservationResponse>());
    }
}