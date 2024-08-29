using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Queries;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationInvoiceTests;

public class GetAllReservationInvoicesHandlerTests
{
    private readonly Mock<IReservationInvoiceRepository> _mockReservationInvoiceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllReservationInvoicesHandler _handler;

    public GetAllReservationInvoicesHandlerTests()
    {
        _mockReservationInvoiceRepository = new Mock<IReservationInvoiceRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllReservationInvoicesHandler(
            _mockReservationInvoiceRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfReservationInvoiceResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllReservationInvoicesRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var reservationInvoices = new List<ReservationInvoices>
        {
            new ReservationInvoices
            {
                Id = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Amount = 300.00m,
                Paid = 150.00m,
                Due = 150.00m,
                Currency = Currencies.USD,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ReservationInvoices
            {
                Id = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Amount = 500.00m,
                Paid = 200.00m,
                Due = 300.00m,
                Currency = Currencies.EUR,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var reservationInvoiceResponses = new List<ReservationInvoiceResponse>
        {
            new ReservationInvoiceResponse
            {
                Id = reservationInvoices[0].Id,
                ReservationId = reservationInvoices[0].ReservationId,
                Amount = reservationInvoices[0].Amount,
                Paid = reservationInvoices[0].Paid,
                Due = reservationInvoices[0].Due,
                Currency = reservationInvoices[0].Currency,
                CreatedAt = reservationInvoices[0].CreatedAt,
                UpdatedAt = reservationInvoices[0].UpdatedAt
            },
            new ReservationInvoiceResponse
            {
                Id = reservationInvoices[1].Id,
                ReservationId = reservationInvoices[1].ReservationId,
                Amount = reservationInvoices[1].Amount,
                Paid = reservationInvoices[1].Paid,
                Due = reservationInvoices[1].Due,
                Currency = reservationInvoices[1].Currency,
                CreatedAt = reservationInvoices[1].CreatedAt,
                UpdatedAt = reservationInvoices[1].UpdatedAt
            }
        };

        _mockReservationInvoiceRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize, CancellationToken.None
            ))
            .ReturnsAsync(reservationInvoices);

        _mockMapper.Setup(m => m.Map<List<ReservationInvoiceResponse>>(reservationInvoices)).Returns(reservationInvoiceResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationInvoiceResponses);
    }
}
