using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.RateTimelines.Common;
using ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class RateTimelinesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RateTimelinesController _controller;

    public RateTimelinesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RateTimelinesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task PushPrice_ReturnsOkResult_WithRateTimelineResponse()
    {
        // Arrange
        var request = new PushPriceRequest(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 150.00m);
        var expectedResponse = new RateTimelineResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RateId = request.RateId,
            RoomTypeId = request.RoomTypeId,
            Date = DateTime.UtcNow,
            Price = request.Price
        };
        var result = Result<RateTimelineResponse>.Success(expectedResponse);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.PushPrice(request);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RateTimelineResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RateTimelineResponse>>()
            .Which.Data.Should().Be(expectedResponse);
    }
}
