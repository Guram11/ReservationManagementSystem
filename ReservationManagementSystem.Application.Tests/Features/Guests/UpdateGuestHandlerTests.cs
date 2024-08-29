using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Guests;

public class UpdateGuestHandlerTests
{
    private readonly Mock<IGuestRepository> _mockGuestRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<UpdateGuestRequest>> _mockValidator;
    private readonly UpdateGuestHandler _handler;

    public UpdateGuestHandlerTests()
    {
        _mockGuestRepository = new Mock<IGuestRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<UpdateGuestRequest>>();
        _handler = new UpdateGuestHandler(_mockGuestRepository.Object, _mockMapper.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenGuestIsUpdated()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var request = new UpdateGuestRequest(guestId, "email@example.com", "John", "Doe", "1234567890");
        var guest = new Guest
        {
            Id = guestId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ReservationRoomId = Guid.NewGuid(),
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
                      .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(m => m.Map<Guest>(request)).Returns(guest);
        _mockMapper.Setup(m => m.Map<GuestResponse>(guest)).Returns(guestResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(guestResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new UpdateGuestRequest(Guid.NewGuid(), "invalidEmail", "John", "Doe", "1234567890");

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                      {
                          new ValidationFailure(nameof(UpdateGuestRequest.Email), "Email is not valid.")
                      }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.ValidationError);
        result.Error.Description.Should().Be("Validation error. Email is not valid.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryUpdateFails()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var request = new UpdateGuestRequest(guestId, "email@example.com", "John", "Doe", "1234567890");
        var guest = new Guest
        {
            Id = guestId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ReservationRoomId = Guid.NewGuid(),
        };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(m => m.Map<Guest>(request)).Returns(guest);
        _mockGuestRepository.Setup(r => r.Update(guestId, guest, CancellationToken.None))
                            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
