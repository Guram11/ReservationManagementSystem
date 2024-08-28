using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class RoomTypesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RoomTypesController _controller;

    public RoomTypesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RoomTypesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithRoomTypeResponses()
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

        var roomTypeResponses = new List<RoomTypeResponse>
            {
                new RoomTypeResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow, HotelId = Guid.NewGuid(), IsActive = true,
                    MaxCapacity = 3, MinCapacity = 1, Name = "RoomType 1", NumberOfRooms = 3 },
                new RoomTypeResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow, HotelId = Guid.NewGuid(), IsActive = true,
                    MaxCapacity = 3, MinCapacity = 1, Name = "RoomType 2", NumberOfRooms = 2 },
            };
        var result = Result<List<RoomTypeResponse>>.Success(roomTypeResponses);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRoomTypesRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roomTypeResponses);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsOkResultWithRoomTypeResponse()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var roomTypeResponse = new RoomTypeResponse
        {
            Id = roomTypeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            IsActive = true,
            MaxCapacity = 3,
            MinCapacity = 1,
            Name = "RoomType1",
            NumberOfRooms = 2           
        };
        var result = Result<RoomTypeResponse>.Success(roomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRoomTypeByIdRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(roomTypeId);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsOkResultWithRoomTypeResponse()
    {
        // Arrange
        var createRoomTypeRequest = new CreateRoomTypeRequest(Guid.NewGuid(), "RoomType1", 2, true, 1, 4);
        var roomTypeResponse = new RoomTypeResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            IsActive = true,
            MaxCapacity = 3,
            MinCapacity = 1,
            Name = "RoomType1",
            NumberOfRooms = 2
        };
        var result = Result<RoomTypeResponse>.Success(roomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(createRoomTypeRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(createRoomTypeRequest);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Update_WhenCalled_ReturnsOkResultWithRoomTypeResponse()
    {
        // Arrange
        var updateRoomTypeRequest = new UpdateRoomTypeRequest(Guid.NewGuid(), Guid.NewGuid(), "RoomType1", 2, true, 1, 4);
        var roomTypeResponse = new RoomTypeResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            IsActive = true,
            MaxCapacity = 3,
            MinCapacity = 1,
            Name = "RoomType1",
            NumberOfRooms = 2
        };
        var result = Result<RoomTypeResponse>.Success(roomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(updateRoomTypeRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(updateRoomTypeRequest);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roomTypeResponse);
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithRoomTypeResponse()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();
        var roomTypeResponse = new RoomTypeResponse
        {
            Id = roomTypeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            IsActive = true,
            MaxCapacity = 3,
            MinCapacity = 1,
            Name = "RoomType1",
            NumberOfRooms = 2
        };
        var result = Result<RoomTypeResponse>.Success(roomTypeResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRoomTypeRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(roomTypeId);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roomTypeResponse);
    }
}
