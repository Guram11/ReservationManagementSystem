using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels;

public class UpdateHotelHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<UpdateHotelRequest>> _mockValidator;
    private readonly UpdateHotelHandler _handler;

    public UpdateHotelHandlerTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<UpdateHotelRequest>>();
        _handler = new UpdateHotelHandler(_mockHotelRepository.Object, _mockMapper.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelIsUpdated()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var request = new UpdateHotelRequest(hotelId, "Valid Hotel Name");
        var hotel = new Hotel { Id = hotelId, Name = "Valid Hotel Name" };
        var hotelResponse = new HotelResponse { Id = hotelId, Name = "Valid Hotel Name" };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(m => m.Map<Hotel>(request)).Returns(hotel);
        _mockMapper.Setup(m => m.Map<HotelResponse>(hotel)).Returns(hotelResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var request = new UpdateHotelRequest(Guid.NewGuid(), "Shor");

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                      {
                          new ValidationFailure(nameof(UpdateHotelRequest.Name), "Name must be at least 5 characters.")
                      }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("ValidationError");
        result.Error.Description.Should().Be("Validation error. Name must be at least 5 characters.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryUpdateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var request = new UpdateHotelRequest(hotelId, "Valid Hotel Name");
        var hotel = new Hotel { Id = hotelId, Name = "Valid Hotel Name" };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(m => m.Map<Hotel>(request)).Returns(hotel);
        _mockHotelRepository.Setup(r => r.Update(hotelId, hotel))
                            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
