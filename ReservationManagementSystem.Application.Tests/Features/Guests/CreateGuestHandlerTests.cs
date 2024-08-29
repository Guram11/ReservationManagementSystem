using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Guests;

public class CreateGuestHandlerTests
{
    private readonly Mock<IGuestRepository> _mockGuestRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<CreateGuestRequest>> _mockValidator;
    private readonly CreateGuestHandler _handler;

    public CreateGuestHandlerTests()
    {
        _mockGuestRepository = new Mock<IGuestRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<CreateGuestRequest>>();
        _handler = new CreateGuestHandler(_mockGuestRepository.Object, _mockMapper.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new CreateGuestRequest("invalidEmail", "John", "Doe", "1234567890", Guid.NewGuid());
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("Email", "Email is not valid.") }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.ValidationError);
        result.Error.Description.Should().Contain("Email is not valid.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailIsAlreadyInUse()
    {
        // Arrange
        var request = new CreateGuestRequest("email@example.com", "John", "Doe", "1234567890", Guid.NewGuid());
        var existingGuest = new Guest { Id = Guid.NewGuid(), Email = request.Email, FirstName = "test", LastName = "test", PhoneNumber = "1234567890" };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockGuestRepository.Setup(repo => repo.GetGuestByEmail(request.Email)).ReturnsAsync(existingGuest);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(ErrorType.AlreadyCreatedError);
        result.Error.Description.Should().Contain("Email is already in use!");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenGuestIsCreated()
    {
        // Arrange
        var request = new CreateGuestRequest("email@example.com", "John", "Doe", "1234567890", Guid.NewGuid());
        var guest = new Guest
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ReservationRoomId = request.ReservationRoomId
        };
        var guestResponse = new GuestResponse
        {
            Id = guest.Id,
            Email = guest.Email,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            PhoneNumber = guest.PhoneNumber,
            ReservationRoomId = guest.ReservationRoomId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };


        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockGuestRepository.Setup(repo => repo.GetGuestByEmail(request.Email)).ReturnsAsync((Guest)null!);
        _mockMapper.Setup(m => m.Map<Guest>(request)).Returns(guest);
        _mockGuestRepository.Setup(repo => repo.Create(guest)).ReturnsAsync(guest);
        _mockMapper.Setup(m => m.Map<GuestResponse>(guest)).Returns(guestResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(guestResponse);
    }
}
