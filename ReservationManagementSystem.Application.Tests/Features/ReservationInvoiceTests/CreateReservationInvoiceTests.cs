using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;
using FluentAssertions;

public class CreateReservationInvoiceHandlerTests
{
    private readonly Mock<IReservationInvoiceRepository> _reservationInvoiceRepositoryMock;
    private readonly Mock<IReservationRoomRepository> _reservationRoomRepositoryMock;
    private readonly Mock<IReservationRoomTimelineRepository> _reservationRoomTimelineRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateReservationInvoiceRequest>> _validatorMock;
    private readonly CreateReservationInvoicelHandler _handler;

    public CreateReservationInvoiceHandlerTests()
    {
        _reservationInvoiceRepositoryMock = new Mock<IReservationInvoiceRepository>();
        _reservationRoomRepositoryMock = new Mock<IReservationRoomRepository>();
        _reservationRoomTimelineRepositoryMock = new Mock<IReservationRoomTimelineRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateReservationInvoiceRequest>>();
        _handler = new CreateReservationInvoicelHandler(
            _reservationInvoiceRepositoryMock.Object,
            _reservationRoomTimelineRepositoryMock.Object,
            _reservationRoomRepositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateReservationInvoiceRequest(
            Guid.NewGuid(),
            300.00m,
            Currencies.USD
        );

        var reservationRoom = new ReservationRoom { Id = Guid.NewGuid(), ReservationId = request.ReservationId };
        var roomTimelines = new List<ReservationRoomTimeline>
        {
            new ReservationRoomTimeline { Id = Guid.NewGuid(), Price = 500.00m, ReservationRoomId = reservationRoom.Id }
        };
        var currencyRate = new CurrencyRate { MainCurrency = "GEL", Rate = 2.5m, Currency = "USD", CurrencyCode = "USD" };

        var reservationInvoice = new ReservationInvoices
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationId = request.ReservationId,
            Amount = 200.00m,
            Paid = request.Paid,
            Due = -100.00m,
            Currency = request.Currency
        };

        var reservationInvoiceResponse = new ReservationInvoiceResponse
        {
            Id = reservationInvoice.Id,
            CreatedAt = reservationInvoice.CreatedAt,
            UpdatedAt = reservationInvoice.UpdatedAt,
            ReservationId = reservationInvoice.ReservationId,
            Amount = reservationInvoice.Amount,
            Paid = reservationInvoice.Paid,
            Due = reservationInvoice.Due,
            Currency = reservationInvoice.Currency
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _reservationRoomRepositoryMock
            .Setup(repo => repo.GetReservationRoomByReservationId(request.ReservationId))
            .ReturnsAsync(reservationRoom);

        _reservationRoomTimelineRepositoryMock
            .Setup(repo => repo.GetReservationRoomTimelinesByReservationRoomId(reservationRoom.Id))
            .ReturnsAsync(roomTimelines);

        _reservationInvoiceRepositoryMock
            .Setup(repo => repo.GetCurrencyRate(request.Currency.ToString()))
            .ReturnsAsync(currencyRate);

        _mapperMock.Setup(m => m.Map<ReservationInvoices>(It.IsAny<ReservationInvoices>())).Returns(reservationInvoice);
        _mapperMock.Setup(m => m.Map<ReservationInvoiceResponse>(reservationInvoice)).Returns(reservationInvoiceResponse);

        _reservationInvoiceRepositoryMock.Setup(repo => repo.Create(reservationInvoice)).ReturnsAsync(reservationInvoice);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationInvoiceResponse);
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationInvoiceRequest(
            Guid.NewGuid(),
            300.00m,
            (Currencies)999
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Currency", "Invalid currency type.")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid currency type.");
    }

    [Fact]
    public async Task Handle_WhenReservationRoomIsNotFound_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationInvoiceRequest(
            Guid.NewGuid(),
            300.00m,
            Currencies.USD
        );

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _reservationRoomRepositoryMock
            .Setup(repo => repo.GetReservationRoomByReservationId(request.ReservationId))
            .ReturnsAsync((ReservationRoom)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("ReservationRoom was not found.");
    }

    [Fact]
    public async Task Handle_WhenCurrencyRateIsNotFound_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateReservationInvoiceRequest(
            Guid.NewGuid(),
            300.00m,
            Currencies.USD
        );

        var reservationRoom = new ReservationRoom { Id = Guid.NewGuid(), ReservationId = request.ReservationId };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _reservationRoomRepositoryMock
            .Setup(repo => repo.GetReservationRoomByReservationId(request.ReservationId))
            .ReturnsAsync(reservationRoom);

        _reservationInvoiceRepositoryMock
            .Setup(repo => repo.GetCurrencyRate(request.Currency.ToString()))
            .ReturnsAsync((CurrencyRate)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("ReservationRoom was not found.");
    }
}