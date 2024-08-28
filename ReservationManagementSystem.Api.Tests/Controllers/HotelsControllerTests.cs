using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class HotelsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly HotelsController _controller;

    public HotelsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new HotelsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithHotelResponses()
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

        var hotelResponses = new List<HotelResponse>
        {
            new HotelResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Name = "Hotel A" },
            new HotelResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Name = "Hotel B" }
        };

        var result = Result<List<HotelResponse>>.Success(hotelResponses);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllHotelsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelResponses);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsOkResultWithHotelResponse()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotelResponse = new HotelResponse
        {
            Id = hotelId,
            Name = "Hotel Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = Result<HotelResponse>.Success(hotelResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetHotelByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(hotelId);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsOkResultWithHotelResponse()
    {
        // Arrange
        var createHotelRequest = new CreateHotelRequest("Hotel A");
        var hotelResponse = new HotelResponse
        {
            Id = Guid.NewGuid(),
            Name = "Hotel A",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = Result<HotelResponse>.Success(hotelResponse);

        _mediatorMock
            .Setup(m => m.Send(createHotelRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createHotelRequest);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Update_WhenCalled_ReturnsOkResultWithHotelResponse()
    {
        // Arrange
        var updateHotelRequest = new UpdateHotelRequest(Guid.NewGuid(), "Hotel B");
        var hotelResponse = new HotelResponse
        {
            Id = Guid.NewGuid(),
            Name = "Hotel B",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = Result<HotelResponse>.Success(hotelResponse);

        _mediatorMock
            .Setup(m => m.Send(updateHotelRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(updateHotelRequest);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithHotelResponse()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotelResponse = new HotelResponse
        {
            Id = hotelId,
            Name = "Deleted Hotel Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = Result<HotelResponse>.Success(hotelResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteHotelRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(hotelId);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsNotFound_WhenHotelIsNotFound()
    {
        // Arrange
        var result = Result<HotelResponse>.Failure(new Error("NotFound", "Hotel not found."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetHotelByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(Guid.NewGuid());

        // Assert
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("Hotel not found.");
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var result = Result<HotelResponse>.Failure(new Error("ValidationError", "Invalid request data."));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateHotelRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(new CreateHotelRequest("Hotel A"));

        // Assert
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Invalid request data.");
    }
}