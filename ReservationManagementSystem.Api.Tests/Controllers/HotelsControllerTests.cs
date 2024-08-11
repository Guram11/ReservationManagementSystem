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

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllHotelsRequest>(), default))
            .ReturnsAsync(hotelResponses);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<HotelResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(hotelResponses);
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
            .Setup(m => m.Send(It.IsAny<GetHotelByIdRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(hotelId);

        // Assert
        var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
        var resultData = okResult.Value.Should().BeOfType<Result<HotelResponse>>().Subject;
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
            .Setup(m => m.Send(createHotelRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createHotelRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<HotelResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<HotelResponse>>();
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
            .Setup(m => m.Send(updateHotelRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(updateHotelRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<HotelResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<HotelResponse>>();
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
            .Setup(m => m.Send(It.IsAny<DeleteHotelRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(hotelId);

        // Assert
        actionResult.Should().BeOfType<ActionResult<HotelResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<HotelResponse>>();
    }
}