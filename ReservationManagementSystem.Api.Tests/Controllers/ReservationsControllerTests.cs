using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Features.Reservations.Queries;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class ReservationsControllerTests
{
    private readonly ReservationsController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public ReservationsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ReservationsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfReservationResponse()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "Number",
            FilterQuery = "123",
            SortBy = "Price",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };
        var reservations = new List<ReservationResponse>
        {
            new ReservationResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelId = Guid.NewGuid(),
                Number = "123",
                Price = 150.00m,
                StatusId = ReservationStatus.Reserved,
                Checkin = DateTime.UtcNow.AddDays(1),
                Checkout = DateTime.UtcNow.AddDays(3),
                Currency = Currencies.USD
            },
            new ReservationResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelId = Guid.NewGuid(),
                Number = "124",
                Price = 200.00m,
                StatusId = ReservationStatus.Created,
                Checkin = DateTime.UtcNow.AddDays(4),
                Checkout = DateTime.UtcNow.AddDays(6),
                Currency = Currencies.USD
            }
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllReservationsRequest>(), default))
            .ReturnsAsync(reservations);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<ReservationResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(reservations);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithReservationResponse()
    {
        // Arrange
        var request = new CreateReservationRequest(Guid.NewGuid(), "123", 150.00m, ReservationStatus.Reserved, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3), Currencies.USD);
        var reservation = new ReservationResponse
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
        var result = Result<ReservationResponse>.Success(reservation);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request);

        // Assert
        actionResult.Should().BeOfType<ActionResult<ReservationResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<ReservationResponse>>()
            .Which.Data.Should().BeEquivalentTo(reservation);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithReservationResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservation = new ReservationResponse
        {
            Id = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            Number = "123",
            Price = 150.00m,
            StatusId = ReservationStatus.Canceled,
            Checkin = DateTime.UtcNow.AddDays(1),
            Checkout = DateTime.UtcNow.AddDays(3),
            Currency = Currencies.USD
        };
        var result = Result<ReservationResponse>.Success(reservation);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteReservationRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(id);

        // Assert
        actionResult.Should().BeOfType<ActionResult<ReservationResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<ReservationResponse>>()
            .Which.Data.Should().BeEquivalentTo(reservation);
    }
}
