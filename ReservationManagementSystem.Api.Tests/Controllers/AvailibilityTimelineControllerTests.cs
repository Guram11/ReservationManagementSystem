using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class AvailabilityTimelineControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AvailabilityController _controller;

    public AvailabilityTimelineControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AvailabilityController(_mediatorMock.Object);
    }

    [Fact]
    public async Task PushAvailability_ReturnsOkResult_WithAvailabilityResponse()
    {
        // Arrange
        var request = new PushAvailabilityRequest(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 2);
        var expectedResponse = new AvailabilityResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoomTypeId = Guid.NewGuid(),
            Available = 2,
            Date = DateTime.UtcNow,
        };
        var result = Result<AvailabilityResponse>.Success(expectedResponse);

        _mediatorMock
            .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.PushAvailability(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task CheckAvailability_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CheckAvailabilityRequest(DateTime.Now, DateTime.Now.AddDays(2));
        var expectedResponse = new List<CheckAvailabilityResponse>
        {
            new CheckAvailabilityResponse { RoomType = "Deluxe", AvailableRooms = 5, TotalPrice = 300 }
        };

        var result = Result<List<CheckAvailabilityResponse>>.Success(expectedResponse);

        _mediatorMock
            .Setup(mediator => mediator.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CheckAvailability(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task PushAvailability_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new PushAvailabilityRequest(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 2);
        var result = Result<AvailabilityResponse>.Failure(new Error(ErrorType.ValidationError, "Invalid request data."));

        _mediatorMock
            .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.PushAvailability(request, CancellationToken.None);

        // Assert
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Invalid request data.");
    }

    [Fact]
    public async Task CheckAvailability_ReturnsNotFound_WhenNoAvailabilityFound()
    {
        // Arrange
        var request = new CheckAvailabilityRequest(DateTime.Now, DateTime.Now.AddDays(2));
        var result = Result<List<CheckAvailabilityResponse>>.Failure(new Error(ErrorType.NotFoundError, "No availability found."));

        _mediatorMock
            .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.CheckAvailability(request, CancellationToken.None);

        // Assert
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("No availability found.");
    }
}