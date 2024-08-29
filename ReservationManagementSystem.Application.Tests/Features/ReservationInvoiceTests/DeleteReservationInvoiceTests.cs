using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.DeleteReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationInvoiceTests;

public class DeleteReservationInvoiceHandlerTests
{
    private readonly Mock<IReservationInvoiceRepository> _mockReservationInvoiceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteReservationInvoiceHandler _handler;

    public DeleteReservationInvoiceHandlerTests()
    {
        _mockReservationInvoiceRepository = new Mock<IReservationInvoiceRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteReservationInvoiceHandler(
            _mockReservationInvoiceRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenReservationInvoiceIsDeleted()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        var reservationInvoice = new ReservationInvoices
        {
            Id = invoiceId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationId = Guid.NewGuid(),
            Amount = 300.00m,
            Paid = 150.00m,
            Due = 150.00m,
            Currency = Currencies.USD
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

        _mockReservationInvoiceRepository.Setup(repo => repo.Delete(invoiceId)).ReturnsAsync(reservationInvoice);
        _mockMapper.Setup(m => m.Map<ReservationInvoiceResponse>(reservationInvoice)).Returns(reservationInvoiceResponse);

        var request = new DeleteReservationInvoiceRequest(invoiceId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationInvoiceResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReservationInvoiceNotFound()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        _mockReservationInvoiceRepository.Setup(repo => repo.Delete(invoiceId)).ReturnsAsync((ReservationInvoices)null!);

        var request = new DeleteReservationInvoiceRequest(invoiceId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.NotFoundError);
        result.Error.Description.Should().Be($"ReservationInvoice with ID {invoiceId} was not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        _mockReservationInvoiceRepository.Setup(repo => repo.Delete(invoiceId)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteReservationInvoiceRequest(invoiceId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
