using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.CreateReservationRoomPayment;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.DeleteReservationRoomPayment;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Queries.GetAllReservationRoomPayments;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class ReservationRoomPaymentsControllerTests
{
    private readonly ReservationRoomPaymentsController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public ReservationRoomPaymentsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReservationRoomPaymentsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfReservationRoomPaymentsResponse()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "ReservationRoomId",
            FilterQuery = "some-reservation-room-id",
            SortBy = "Amount",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };
        var payments = new List<ReservationRoomPaymentsResponse>
        {
            new ReservationRoomPaymentsResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationRoomId = Guid.NewGuid(),
                Amount = 150.00m,
                Description = "Payment 1",
                Currency = Currencies.USD
            },
            new ReservationRoomPaymentsResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationRoomId = Guid.NewGuid(),
                Amount = 200.00m,
                Description = "Payment 2",
                Currency = Currencies.EUR
            }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationRoomPaymentsRequest>(), default))
            .ReturnsAsync(payments);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<ReservationRoomPaymentsResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(payments);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithReservationRoomPaymentsResponse()
    {
        // Arrange
        var request = new CreateReservationRoomPaymentRequest(Guid.NewGuid(), 150.00m, "Payment Description", Currencies.USD);
        var payment = new ReservationRoomPaymentsResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = request.ReservationRoomId,
            Amount = request.Amount,
            Description = request.Description,
            Currency = request.Currency
        };
        var result = Result<ReservationRoomPaymentsResponse>.Success(payment);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request);

        // Assert
        actionResult.Should().BeOfType<ActionResult<ReservationRoomPaymentsResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<ReservationRoomPaymentsResponse>>();
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithReservationRoomPaymentsResponse()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment = new ReservationRoomPaymentsResponse
        {
            Id = paymentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = Guid.NewGuid(),
            Amount = 150.00m,
            Description = "Payment Description",
            Currency = Currencies.USD
        };
        var result = Result<ReservationRoomPaymentsResponse>.Success(payment);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationRoomPaymentRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(paymentId);

        // Assert
        actionResult.Should().BeOfType<ActionResult<ReservationRoomPaymentsResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<ReservationRoomPaymentsResponse>>();
    }
}
