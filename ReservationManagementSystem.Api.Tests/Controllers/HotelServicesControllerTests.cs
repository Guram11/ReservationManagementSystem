using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Features.HotelServices.Queries;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class HotelServiceControllerTests
{
    private readonly HotelServicesController _controller;
    private readonly Mock<IMediator> _mediatorMock;

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
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllHotelServicesRequest>(), default))
            .ReturnsAsync(hotelServices);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<HotelServiceResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(hotelServices);
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
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(request);

        // Assert
        actionResult.Should().BeOfType<ActionResult<HotelServiceResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<HotelServiceResponse>>();
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
            .Setup(m => m.Send(It.IsAny<DeleteHotelServiceRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(id);

        // Assert
        actionResult.Should().BeOfType<ActionResult<HotelServiceResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<HotelServiceResponse>>();
    }
}
