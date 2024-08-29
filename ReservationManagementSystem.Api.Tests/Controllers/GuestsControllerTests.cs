using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Enums;
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
    public async Task GetAll_ReturnsOkResultWithListOfGuests_WhenRequestIsSuccessful()
    {
        // Arrange
        var guests = new List<GuestResponse>
        {
            new GuestResponse { Id = Guid.NewGuid(), PhoneNumber = "123", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new GuestResponse { Id = Guid.NewGuid(), PhoneNumber = "123", FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };

        var result = Result<List<GuestResponse>>.Success(guests);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllGuestsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(new GetAllQueryParams());

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(guests);
    }

    [Fact]
    public async Task Get_ReturnsOkResultWithGuest_WhenRequestIsSuccessful()
    {
        // Arrange
        var guest = new GuestResponse { Id = Guid.NewGuid(), FirstName = "John", PhoneNumber = "123", LastName = "Doe", Email = "john.doe@example.com" };
        var result = Result<GuestResponse>.Success(guest);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetGuestByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(guest.Id);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(guest);
    }

    [Fact]
    public async Task Create_ReturnsOkResultWithCreatedGuest_WhenRequestIsSuccessful()
    {
        // Arrange
        var guest = new GuestResponse { Id = Guid.NewGuid(), FirstName = "John", PhoneNumber = "123", LastName = "Doe", Email = "john.doe@example.com" };
        var result = Result<GuestResponse>.Success(guest);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateGuestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(new CreateGuestRequest("guest1", "guest", "guest", "123", Guid.NewGuid()));

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(guest);
    }

    [Fact]
    public async Task Update_ReturnsOkResultWithUpdatedGuest_WhenRequestIsSuccessful()
    {
        // Arrange
        var guest = new GuestResponse { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = "123", Email = "john.doe@example.com" };
        var result = Result<GuestResponse>.Success(guest);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateGuestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(new UpdateGuestRequest(Guid.NewGuid(), "email@mail.com", "guest1", "guest", "123"));

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(guest);
    }

    [Fact]
    public async Task Delete_ReturnsOkResultWithDeletedGuest_WhenRequestIsSuccessful()
    {
        // Arrange
        var guest = new GuestResponse { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", PhoneNumber = "123", Email = "john.doe@example.com" };
        var result = Result<GuestResponse>.Success(guest);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteGuestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(guest.Id);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(guest);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenGuestIsNotFound()
    {
        // Arrange
        var result = Result<GuestResponse>.Failure(new Error(ErrorType.NotFoundError, "Guest not found."));

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetGuestByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(Guid.NewGuid());

        // Assert
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("Guest not found.");
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var result = Result<GuestResponse>.Failure(new Error(ErrorType.ValidationError, "Invalid request data."));

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateGuestRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Create(new CreateGuestRequest("guest1", "guest", "guest", "123", Guid.NewGuid()));

        // Assert
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Be("Invalid request data.");
    }
}