using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;
using ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class GuestsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GuestsController _controller;

    public GuestsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GuestsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithGuestResponses()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "Email",
            FilterQuery = "example@example.com",
            SortBy = "LastName",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };

        var guestResponses = new List<GuestResponse>
        {
            new GuestResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationRoomId = Guid.NewGuid(),
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123-456-7890"
            },
            new GuestResponse
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ReservationRoomId = Guid.NewGuid(),
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "098-765-4321"
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllGuestsRequest>(), default))
            .ReturnsAsync(guestResponses);

        // Act
        var result = await _controller.GetAll(queryParams);

        // Assert
        result.Should().BeOfType<ActionResult<List<GuestResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(guestResponses);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsOkResultWithGuestResponse()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guestResponse = new GuestResponse
        {
            Id = guestId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = Guid.NewGuid(),
            Email = "john.doe@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "123-456-7890"
        };
        var result = Result<GuestResponse>.Success(guestResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGuestByIdRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(guestId);

        // Assert
        var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
        var resultData = okResult.Value.Should().BeOfType<Result<GuestResponse>>().Subject;
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsOkResultWithGuestResponse()
    {
        // Arrange
        var createGuestRequest = new CreateGuestRequest(
            Email: "john.doe@example.com",
            FirstName: "John",
            LastName: "Doe",
            PhoneNumber: "123-456-7890",
            ReservationRoomId: Guid.NewGuid()
        );

        var guestResponse = new GuestResponse
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = createGuestRequest.ReservationRoomId,
            Email = createGuestRequest.Email,
            FirstName = createGuestRequest.FirstName,
            LastName = createGuestRequest.LastName,
            PhoneNumber = createGuestRequest.PhoneNumber
        };

        _mediatorMock
            .Setup(m => m.Send(createGuestRequest, default))
            .ReturnsAsync(Result<GuestResponse>.Success(guestResponse));

        // Act
        var actionResult = await _controller.Create(createGuestRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<GuestResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(Result<GuestResponse>.Success(guestResponse));
    }

    [Fact]
    public async Task Update_WhenCalled_ReturnsOkResultWithGuestResponse()
    {
        // Arrange
        var updateGuestRequest = new UpdateGuestRequest(
            Id: Guid.NewGuid(),
            Email: "john.updated@example.com",
            FirstName: "John",
            LastName: "Doe",
            PhoneNumber: "987-654-3210"
         );

        var guestResponse = new GuestResponse
        {
            Id = updateGuestRequest.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = Guid.NewGuid(),
            Email = updateGuestRequest.Email,
            FirstName = updateGuestRequest.FirstName,
            LastName = updateGuestRequest.LastName,
            PhoneNumber = updateGuestRequest.PhoneNumber
        };

        _mediatorMock
            .Setup(m => m.Send(updateGuestRequest, default))
            .ReturnsAsync(Result<GuestResponse>.Success(guestResponse));

        // Act
        var actionResult = await _controller.Update(updateGuestRequest);

        // Assert
        actionResult.Should().BeOfType<ActionResult<GuestResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(Result<GuestResponse>.Success(guestResponse));
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithGuestResponse()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guestResponse = new GuestResponse
        {
            Id = guestId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ReservationRoomId = Guid.NewGuid(),
            Email = "deleted.guest@example.com",
            FirstName = "Deleted",
            LastName = "Guest",
            PhoneNumber = "000-000-0000"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteGuestRequest>(), default))
            .ReturnsAsync(Result<GuestResponse>.Success(guestResponse));

        // Act
        var actionResult = await _controller.Delete(guestId);

        // Assert
        actionResult.Should().BeOfType<ActionResult<GuestResponse>>()
            .Which.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(Result<GuestResponse>.Success(guestResponse));
    }
}