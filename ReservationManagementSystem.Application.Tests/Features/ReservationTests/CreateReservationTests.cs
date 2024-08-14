using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationTests;

public class CreateReservationHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateReservationRequest>> _validatorMock;
    private readonly CreateReservationlHandler _handler;

    public CreateReservationHandlerTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateReservationRequest>>();
        _handler = new CreateReservationlHandler(_reservationRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateReservationRequest(
            Guid.NewGuid(),
            "12345",
            200.00m,
            ReservationStatus.Reserved,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            Currencies.USD
        );

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = request.HotelId,
            Number = request.Number,
            Price = request.Price,
            StatusId = request.StatusId,
            Checkin = request.Checkin,
            Checkout = request.Checkout,
            Currency = request.Currency
        };

        var reservationResponse = new ReservationResponse
        {
            Id = reservation.Id,
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt,
            HotelId = reservation.HotelId,
            Number = reservation.Number,
            Price = reservation.Price,
            StatusId = reservation.StatusId,
            Checkin = reservation.Checkin,
            Checkout = reservation.Checkout,
            Currency = reservation.Currency
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
           .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<Reservation>(request)).Returns(reservation);
        _mapperMock.Setup(m => m.Map<ReservationResponse>(reservation)).Returns(reservationResponse);

        _reservationRepositoryMock.Setup(repo => repo.Create(reservation)).ReturnsAsync(reservation);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationResponse);
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationRequest(
            Guid.NewGuid(),
            "12345",
            200.00m,
            (ReservationStatus)999,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            (Currencies)1
        );

        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("StatusId", "Invalid status type."),
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid status type.");
    }
}
