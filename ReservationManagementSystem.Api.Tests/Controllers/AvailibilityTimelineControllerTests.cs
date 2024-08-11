using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.PushAvailability;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class AvailibilityTimelineControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AvailabilityController _controller;

    public AvailibilityTimelineControllerTests()
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
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.PushAvailability(request);

        // Assert
        actionResult.Should().BeOfType<ActionResult<AvailabilityResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<AvailabilityResponse>>();
    }
}
