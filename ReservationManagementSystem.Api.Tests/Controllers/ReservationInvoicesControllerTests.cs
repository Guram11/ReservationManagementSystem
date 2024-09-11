using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.DeleteReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Queries;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class ReservationInvoicesControllerTests
{
    private readonly ReservationInvoicesController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public ReservationInvoicesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReservationInvoicesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfReservationInvoiceResponse()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "Number",
            FilterQuery = "123",
            SortBy = "TotalAmount",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };

        var reservationInvoices = new List<ReservationInvoiceResponse>
        {
            new ReservationInvoiceResponse
            {
                Id = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Amount = 2,
                Due = 3,
                Paid = 4,   
                Currency = Currencies.USD,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ReservationInvoiceResponse
            {
                Id = Guid.NewGuid(),
                ReservationId = Guid.NewGuid(),
                Amount = 2,
                Due = 3,
                Paid = 4,
                Currency = Currencies.USD,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        var result = Result<List<ReservationInvoiceResponse>>.Success(reservationInvoices);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationInvoicesRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<List<ReservationInvoiceResponse>>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(reservationInvoices);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithReservationInvoiceResponse()
    {
        // Arrange
        var request = new CreateReservationInvoiceRequest(Guid.NewGuid(), 2, Currencies.USD);
        var reservationInvoiceResponse = new ReservationInvoiceResponse
        {
            Id = Guid.NewGuid(),
            ReservationId = Guid.NewGuid(),
            Amount = 2,
            Due = 3,
            Paid = 4,
            Currency = Currencies.USD,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = Result<ReservationInvoiceResponse>.Success(reservationInvoiceResponse);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<ReservationInvoiceResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(reservationInvoiceResponse);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithReservationInvoiceResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservationInvoiceResponse = new ReservationInvoiceResponse
        {
            Id = Guid.NewGuid(),
            ReservationId = Guid.NewGuid(),
            Amount = 2,
            Due = 3,
            Paid = 4,
            Currency = Currencies.USD,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = Result<ReservationInvoiceResponse>.Success(reservationInvoiceResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationInvoiceRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(id, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<ReservationInvoiceResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(reservationInvoiceResponse);
    }
}
