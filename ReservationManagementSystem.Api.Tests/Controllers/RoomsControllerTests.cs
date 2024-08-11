using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;
using ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class RoomsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RoomsController _controller;

    public RoomsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RoomsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithRoomResponses()
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

        var roomResponses = new List<RoomResponse>
            {
                new RoomResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow, Number = "112", Floor = 1, Note = "None", RoomTypeId = Guid.NewGuid() },
                new RoomResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow, Number = "113", Floor = 1, Note = "None", RoomTypeId = Guid.NewGuid() }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRoomsRequest>(), default))
            .ReturnsAsync(roomResponses);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<RoomResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(roomResponses);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsOkResultWithRoomResponse()
    {
        // Arrange
        var roomlId = Guid.NewGuid();
        var roomResponse = new RoomResponse
        {
            Id = roomlId,
            Number = "111",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoomTypeId = Guid.NewGuid(),
            Floor = 1,
            Note = "None",
        };
        var result = Result<RoomResponse>.Success(roomResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoomByIdRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(roomlId);

        // Assert
        var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
        var resultData = okResult.Value.Should().BeOfType<Result<RoomResponse>>().Subject;
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsOkResultWithRoomResponse()
    {
        // Arrange
        var createRoomRequest = new CreateRoomRequest(Guid.NewGuid(), "112", 1, "None");
        var roomResponse = new RoomResponse
        {
            Id = Guid.NewGuid(),
            Number = "111",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoomTypeId = Guid.NewGuid(),
            Floor = 1,
            Note = "None",
        };
        var result = Result<RoomResponse>.Success(roomResponse);

        _mediatorMock
            .Setup(m => m.Send(createRoomRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createRoomRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RoomResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RoomResponse>>();
    }

    [Fact]
    public async Task Update_WhenCalled_ReturnsOkResultWithRoomResponse()
    {
        // Arrange
        var updateRoomRequest = new UpdateRoomRequest(Guid.NewGuid(), Guid.NewGuid(), "112", 1, "None");
        var roomResponse = new RoomResponse
        {
            Id = Guid.NewGuid(),
            Number = "111",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoomTypeId = Guid.NewGuid(),
            Floor = 1,
            Note = "None",
        };
        var result = Result<RoomResponse>.Success(roomResponse);

        _mediatorMock
            .Setup(m => m.Send(updateRoomRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(updateRoomRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RoomResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RoomResponse>>();
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithRoomResponse()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomResponse = new RoomResponse
        {
            Id = roomId,
            Number = "111",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoomTypeId = Guid.NewGuid(),
            Floor = 1,
            Note = "None",
        };
        var result = Result<RoomResponse>.Success(roomResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRoomRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(roomId);

        // Assert
        actionResult.Should().BeOfType<ActionResult<RoomResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<Result<RoomResponse>>();
    }
}
