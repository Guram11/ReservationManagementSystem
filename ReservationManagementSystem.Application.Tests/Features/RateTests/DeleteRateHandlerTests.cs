using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateTests;

public class DeleteRateHandlerTests
{
    private readonly Mock<IRateRepository> _rateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteRateHandler _handler;

    public DeleteRateHandlerTests()
    {
        _rateRepositoryMock = new Mock<IRateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteRateHandler(_rateRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RateDeletedSuccessfully_ReturnsSuccessResult()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var rate = new Rate { Id = rateId, Name = "Standard", HotelId = Guid.NewGuid() };

        var request = new DeleteRateRequest(rateId);

        _rateRepositoryMock.Setup(r => r.Delete(rateId, CancellationToken.None))
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
        var request = new DeleteRateRequest(rateId);

        _rateRepositoryMock.Setup(r => r.Delete(rateId, CancellationToken.None))
            .ReturnsAsync((Rate)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be($"Rate with ID {rateId} was not found.");
    }
}
