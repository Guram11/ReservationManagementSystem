using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels.Commands.CreateHotel;

public class CreateHotelHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<CreateHotelRequest>> _mockValidator;
    private readonly CreateHotelHandler _handler;

    public CreateHotelHandlerTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<CreateHotelRequest>>();
        _handler = new CreateHotelHandler(_mockHotelRepository.Object, _mockMapper.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Validation_Fails()
    {
        // Arrange
        var request = new CreateHotelRequest("H");
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("Name", "Name must be at least 5 characters.") }));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("ValidationFailed");
        result.Error.Description.Should().Contain("Name must be at least 5 characters.");
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Hotel_Is_Created()
    {
        // Arrange
        var request = new CreateHotelRequest("Valid Hotel Name");
        var hotel = new Hotel { Id = Guid.NewGuid(), Name = request.Name };
        var hotelResponse = new HotelResponse { Id = hotel.Id, Name = hotel.Name };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockMapper.Setup(m => m.Map<Hotel>(request)).Returns(hotel);
        _mockMapper.Setup(m => m.Map<HotelResponse>(hotel)).Returns(hotelResponse);

        _mockHotelRepository.Setup(repo => repo.Create(hotel)).ReturnsAsync(hotel);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelResponse);
    }
}
