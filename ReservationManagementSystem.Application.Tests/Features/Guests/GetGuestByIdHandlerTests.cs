using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Guests;

public class GetGuestByIdHandlerTests
{
    private readonly Mock<IGuestRepository> _mockGuestRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetGuestByIdHandler _handler;

    public GetGuestByIdHandlerTests()
    {
        _mockGuestRepository = new Mock<IGuestRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetGuestByIdHandler(_mockGuestRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenGuestIsFound()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = new Guest { Id = guestId, Email = "test@example.com", FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890", ReservationRoomId = Guid.NewGuid() };
        var guestResponse = new GuestResponse { Id = guestId, Email = guest.Email, FirstName = guest.FirstName, LastName = guest.LastName, PhoneNumber = guest.PhoneNumber, ReservationRoomId = guest.ReservationRoomId };

        _mockGuestRepository.Setup(repo => repo.Get(guestId)).ReturnsAsync(guest);
        _mockMapper.Setup(m => m.Map<GuestResponse>(guest)).Returns(guestResponse);

        var request = new GetGuestByIdRequest(guestId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(guestResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenGuestNotFound()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        _mockGuestRepository.Setup(repo => repo.Get(guestId)).ReturnsAsync((Guest)null!);

        var request = new GetGuestByIdRequest(guestId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.NotFoundError);
        result.Error.Description.Should().Be($"Guest with ID {guestId} was not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        _mockGuestRepository.Setup(repo => repo.Get(guestId)).ThrowsAsync(new Exception("Database error"));

        var request = new GetGuestByIdRequest(guestId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
