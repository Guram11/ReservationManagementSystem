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

namespace ReservationManagementSystem.Application.Tests.Features.ReservationInvoiceTests;

public class CreateReservationInvoiceHandlerTests
{
    private readonly Mock<IReservationInvoiceRepository> _reservationInvoiceRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateReservationInvoiceRequest>> _validatorMock;
    private readonly CreateReservationInvoicelHandler _handler;

    public CreateReservationInvoiceHandlerTests()
    {
        _reservationInvoiceRepositoryMock = new Mock<IReservationInvoiceRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateReservationInvoiceRequest>>();
        _handler = new CreateReservationInvoicelHandler(
            _reservationInvoiceRepositoryMock.Object,
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
            150.00m,
            150.00m,
            Currencies.USD
        );

        var reservationInvoice = new ReservationInvoices
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationId = request.ReservationId,
            Amount = request.Amount,
            Paid = request.Paid,
            Due = request.Due,
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

        _mapperMock.Setup(m => m.Map<ReservationInvoices>(request)).Returns(reservationInvoice);
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
            150.00m,
            150.00m,
            (Currencies)999 // Invalid currency
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
}
