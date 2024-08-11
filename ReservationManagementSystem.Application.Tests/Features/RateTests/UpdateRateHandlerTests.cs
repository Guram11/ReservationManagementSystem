using AutoMapper;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using FluentAssertions;

namespace ReservationManagementSystem.Application.Tests.Features.RateTests;

public class UpdateRateHandlerTests
{
    private readonly Mock<IRateRepository> _rateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<UpdateRateRequest>> _validatorMock;
    private readonly UpdateRateHandler _handler;

    public UpdateRateHandlerTests()
    {
        _rateRepositoryMock = new Mock<IRateRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<UpdateRateRequest>>();
        _handler = new UpdateRateHandler(_rateRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new UpdateRateRequest(Guid.NewGuid(), "Standard", Guid.NewGuid());
        var rate = new Rate { Id = request.Id, Name = request.Name, HotelId = request.HotelID };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock.Setup(m => m.Map<Rate>(request))
            .Returns(rate);

        var rateResponse = new RateResponse { Id = rate.Id, Name = rate.Name, HotelId = rate.HotelId };

        _mapperMock.Setup(m => m.Map<RateResponse>(rate))
            .Returns(rateResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(rateResponse);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailureResult()
    {
        // Arrange
        var request = new UpdateRateRequest(Guid.NewGuid(), "", Guid.NewGuid());
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name must be at least 3 characters.")
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be("Validation error. Name must be at least 3 characters.");
    }
}
