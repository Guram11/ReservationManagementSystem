using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;
using ReservationManagementSystem.Application.Wrappers;
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

        var getAllRateRoomTypesRequest = new GetAllRateRoomTypesRequest(queryParams.FilterOn, queryParams.FilterQuery, queryParams.SortBy,
            queryParams.IsAscending, queryParams.PageNumber, queryParams.PageSize);

        var rateRoomTypeResponses = new List<RateRoomTypeResponse>
            {
                new RateRoomTypeResponse
                {
                   RateId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, RoomTypeId = Guid.NewGuid(), UpdatedAt = DateTime.UtcNow,
                },
                 new RateRoomTypeResponse
                {
                   RateId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, RoomTypeId = Guid.NewGuid(), UpdatedAt = DateTime.UtcNow,
                },
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRateRoomTypesRequest>(), default))
            .ReturnsAsync(rateRoomTypeResponses);

        // Act
        var actionResult = await _controller.GetAll(queryParams);

        // Assert
        actionResult.Should().BeOfType<ActionResult<List<RateRoomTypeResponse>>>();

        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(rateRoomTypeResponses);
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsOkResultWithRateRoomTypeResponse()
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
            .Setup(m => m.Send(createRateRoomTypeRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createRateRoomTypeRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RateRoomTypeResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RateRoomTypeResponse>>();
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithRateRoomTypeResponse()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var rateId = Guid.NewGuid();
        var rateRoomTypeResponse = new RateRoomTypeResponse
        {
            RateId = roomTypeId,
            RoomTypeId = roomTypeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        var result = Result<RateRoomTypeResponse>.Success(rateRoomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRateRoomTypeRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(rateId, roomTypeId);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RateRoomTypeResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RateRoomTypeResponse>>();
    }
}
