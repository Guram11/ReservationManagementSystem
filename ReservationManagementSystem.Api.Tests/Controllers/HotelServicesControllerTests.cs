using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Features.HotelServices.Queries;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class HotelServiceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly HotelServicesController _controller;

    public HotelServiceControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new HotelServicesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfHotelServiceResponse()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "ServiceType",
            FilterQuery = "Spa",
            SortBy = "Price",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };

        var hotelServices = new List<HotelServiceResponse>
        {
            new HotelServiceResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelId = Guid.NewGuid(),
                ServiceTypeId = HotelServiceTypes.MiniBar,
                Description = "Relaxing spa treatment",
                Price = 100.00m
            },
            new HotelServiceResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HotelId = Guid.NewGuid(),
                ServiceTypeId = HotelServiceTypes.HotelDamage,
                Description = "Fully equipped gym",
                Price = 50.00m
            }
        };
        var result = Result<List<HotelServiceResponse>>.Success(hotelServices);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllHotelServicesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelServices);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithHotelServiceResponse()
    {
        // Arrange
        var request = new CreateHotelServiceRequest(Guid.NewGuid(), HotelServiceTypes.MiniBar, "desc", 10.00m);
        var hotelService = new HotelServiceResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = request.HotelId,
            ServiceTypeId = request.ServiceTypeId,
            Description = request.Description,
            Price = request.Price
        };
        var result = Result<HotelServiceResponse>.Success(hotelService);
        _mediatorMock
            .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelService);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithHotelServiceResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var hotelService = new HotelServiceResponse
        {
            Id = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            ServiceTypeId = HotelServiceTypes.MiniBar,
            Description = "Hotel pool access",
            Price = 20.00m
        };
        var result = Result<HotelServiceResponse>.Success(hotelService);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteHotelServiceRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(id, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelService);
    }

    [Fact]
    public async Task GetAll_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = null,
            FilterQuery = null,
            SortBy = null,
            IsAscending = true,
            PageNumber = 0,
            PageSize = 0
        };
        var result = Result<List<HotelServiceResponse>>.Failure(new Error(ErrorType.ValidationError, "Invalid request parameters."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllHotelServicesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Invalid request parameters.");
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenServiceDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var result = Result<HotelServiceResponse>.Failure(new Error(ErrorType.NotFoundError, "Service not found."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteHotelServiceRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(id, CancellationToken.None);

        // Assert
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("Service not found.");
    }
}
