﻿using FluentAssertions;
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
        var result = Result<List<ReservationRoomPaymentsResponse>>.Success(payments);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationRoomPaymentsRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<List<ReservationRoomPaymentsResponse>>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(payments);
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
        var actionResult = await _controller.Create(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<ReservationRoomPaymentsResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(payment);
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
        var actionResult = await _controller.Delete(paymentId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<ReservationRoomPaymentsResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(payment);
    }
}
