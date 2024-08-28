using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class RateRoomTypeControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RateRoomTypesController _controller;

    public RateRoomTypeControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RateRoomTypesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithRateRoomTypeResponses()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "Name",
            FilterQuery = "Test",
            SortBy = "Name",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };

        var rateRoomTypeResponses = new List<RateRoomTypeResponse>
        {
            new RateRoomTypeResponse
            {
                RateId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                RoomTypeId = Guid.NewGuid(),
                UpdatedAt = DateTime.UtcNow,
            },
            new RateRoomTypeResponse
            {
                RateId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                RoomTypeId = Guid.NewGuid(),
                UpdatedAt = DateTime.UtcNow,
            },
        };
        var result = Result<List<RateRoomTypeResponse>>.Success(rateRoomTypeResponses);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRateRoomTypesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(rateRoomTypeResponses);
    }

    [Fact]
    public async Task Create_ShouldReturnOkResult_WithRateRoomTypeResponse()
    {
        // Arrange
        var createRateRoomTypeRequest = new CreateRateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid());
        var rateRoomTypeResponse = new RateRoomTypeResponse
        {
            RateId = Guid.NewGuid(),
            RoomTypeId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        var result = Result<RateRoomTypeResponse>.Success(rateRoomTypeResponse);
        _mediatorMock
            .Setup(m => m.Send(createRateRoomTypeRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createRateRoomTypeRequest);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(rateRoomTypeResponse);
    }

    [Fact]
    public async Task Delete_ShouldReturnOkResult_WithRateRoomTypeResponse()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var rateId = Guid.NewGuid();
        var rateRoomTypeResponse = new RateRoomTypeResponse
        {
            RateId = rateId,
            RoomTypeId = roomTypeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        var result = Result<RateRoomTypeResponse>.Success(rateRoomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRateRoomTypeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(rateId, roomTypeId);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(rateRoomTypeResponse);
    }

    [Fact]
    public async Task GetAll_ShouldReturnBadRequest_WhenValidationFails()
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

        var result = Result<List<RateRoomTypeResponse>>.Failure(new Error("ValidationError", "Invalid request parameters."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRateRoomTypesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams);

        // Assert
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Invalid request parameters.");
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenRateRoomTypeDoesNotExist()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var rateId = Guid.NewGuid();
        var result = Result<RateRoomTypeResponse>.Failure(new Error("NotFound", "RateRoomType not found."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRateRoomTypeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(rateId, roomTypeId);

        // Assert
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("RateRoomType not found.");
    }
}
