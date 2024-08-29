using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateTests;

public class GetRateByIdHandlerTests
{
    private readonly Mock<IRateRepository> _rateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetRateByIdHandler _handler;

    public GetRateByIdHandlerTests()
    {
        _rateRepositoryMock = new Mock<IRateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetRateByIdHandler(_rateRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var rate = new Rate { Id = rateId, Name = "Standard", HotelId = Guid.NewGuid() };

        var request = new GetRateByIdRequest(rateId);

        _rateRepositoryMock.Setup(r => r.Get(rateId))
            .ReturnsAsync(rate);

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
    public async Task Handle_RateNotFound_ReturnsFailureResult()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        _rateRepositoryMock.Setup(repo => repo.Get(rateId))
            .ReturnsAsync((Rate)null!);

        var request = new GetRateByIdRequest(rateId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.NotFoundError);
        result.Error.Description.Should().Be($"Rate with ID {rateId} was not found.");
    }
}
