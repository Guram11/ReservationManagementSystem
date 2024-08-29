using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.HotelServices;

public class CreateHotelServiceTests
{
    private readonly Mock<IHotelServiceRepository> _hotelServiceRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateHotelServiceRequest>> _validatorMock;
    private readonly CreateHotelServicelHandler _handler;

    public CreateHotelServiceTests()
    {
        _hotelServiceRepositoryMock = new Mock<IHotelServiceRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateHotelServiceRequest>>();
        _handler = new CreateHotelServicelHandler(_hotelServiceRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateHotelServiceRequest(
            Guid.NewGuid(),
            HotelServiceTypes.MiniBar,
            "Relaxing spa treatment",
            100.00m
        );
        var hotelService = new HotelService
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = request.HotelId,
            ServiceTypeId = request.ServiceTypeId,
            Description = request.Description,
            Price = request.Price
        };
        var hotelServiceResponse = new HotelServiceResponse
        {
            Id = hotelService.Id,
            CreatedAt = hotelService.CreatedAt,
            UpdatedAt = hotelService.UpdatedAt,
            HotelId = hotelService.HotelId,
            ServiceTypeId = hotelService.ServiceTypeId,
            Description = hotelService.Description,
            Price = hotelService.Price
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mapperMock.Setup(m => m.Map<HotelService>(request)).Returns(hotelService);
        _mapperMock.Setup(m => m.Map<HotelServiceResponse>(hotelService)).Returns(hotelServiceResponse);

        _hotelServiceRepositoryMock.Setup(repo => repo.Create(hotelService, CancellationToken.None)).ReturnsAsync(hotelService);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelServiceResponse);
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateHotelServiceRequest(
            Guid.NewGuid(),
            (HotelServiceTypes)999,
            "Invalid service",
            10.00m
        );

        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("ServiceTypeId", "Invalid service type."),
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Contain("Invalid service type.");
    }
}
