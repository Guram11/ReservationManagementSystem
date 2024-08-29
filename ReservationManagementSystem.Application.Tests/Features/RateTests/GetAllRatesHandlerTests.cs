using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetAllRates;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateTests;

public class GetAllRatesHandlerTests
{
    private readonly Mock<IRateRepository> _rateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllRatesHandler _handler;

    public GetAllRatesHandlerTests()
    {
        _rateRepositoryMock = new Mock<IRateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllRatesHandler(_rateRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsRateResponses()
    {
        // Arrange
        var rates = new List<Rate>
        {
            new Rate { Id = Guid.NewGuid(), Name = "Standard", HotelId = Guid.NewGuid() },
            new Rate { Id = Guid.NewGuid(), Name = "Deluxe", HotelId = Guid.NewGuid() }
        };

        var request = new GetAllRatesRequest(null, null, null, true, 1, 10);

        _rateRepositoryMock.Setup(x => x.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, CancellationToken.None))
            .ReturnsAsync(rates);

        var rateResponses = new List<RateResponse>
        {
            new RateResponse { Id = rates[0].Id, Name = rates[0].Name, HotelId = rates[0].HotelId },
            new RateResponse { Id = rates[1].Id, Name = rates[1].Name, HotelId = rates[1].HotelId }
        };

        _mapperMock.Setup(x => x.Map<List<RateResponse>>(rates))
            .Returns(rateResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(rateResponses);
    }

    [Fact]
    public async Task Handle_EmptyResult_ReturnsEmptyList()
    {
        // Arrange
        var rates = new List<Rate>();

        var request = new GetAllRatesRequest(null, null, null, true, 1, 10);

        _rateRepositoryMock.Setup(x => x.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, CancellationToken.None))
            .ReturnsAsync(rates);

        var rateResponses = new List<RateResponse>();

        _mapperMock.Setup(x => x.Map<List<RateResponse>>(rates))
            .Returns(rateResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
    }
}
