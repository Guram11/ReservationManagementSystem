using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;
using ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;
using ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetAllRates;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class RatesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RatesController _controller;

    public RatesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RatesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithRateResponses()
    {
        // Arrange
        var queryParams = new GetAllQueryParams
        {
            FilterOn = "Name",
            FilterQuery = "Test",
            SortBy = "Name",
            IsAscending = true,
            PageNumber = 1,
            PageSize = 10
        };

        var rateResponses = new List<RateResponse>
        {
            new RateResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Name = "Hotel A", HotelId = Guid.NewGuid() },
            new RateResponse { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Name = "Hotel B", HotelId = Guid.NewGuid() }
        };
        var result = Result<List<RateResponse>>.Success(rateResponses);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllRatesRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.GetAll(queryParams, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<List<RateResponse>>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(rateResponses);
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsOkResultWithRateResponse()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var rateResponse = new RateResponse
        {
            Id = rateId,
            Name = "Hotel Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid()
        };
        var result = Result<RateResponse>.Success(rateResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetRateByIdRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Get(rateId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<RateResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(rateResponse);
    }

[Fact]
public async Task Create_WhenCalled_ReturnsOkResultWithRateResponse()
{
    // Arrange
    var createRateRequest = new CreateRateRequest("Rate A", Guid.NewGuid());
    var rateResponse = new RateResponse
    {
        Id = Guid.NewGuid(),
        Name = "Rate A",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        HotelId = Guid.NewGuid()
    };
    var result = Result<RateResponse>.Success(rateResponse);

    _mediatorMock
        .Setup(m => m.Send(createRateRequest, default))
        .ReturnsAsync(result);

    // Act
    var actionResult = await _controller.Create(createRateRequest, CancellationToken.None);

    // Assert
    var okResult = actionResult.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    
    var responseResult = okResult!.Value as Result<RateResponse>;
    responseResult.Should().NotBeNull();
    responseResult!.Data.Should().BeEquivalentTo(rateResponse);
}

    [Fact]
    public async Task Update_WhenCalled_ReturnsOkResultWithRateResponse()
    {
        // Arrange
        var updateRateRequest = new UpdateRateRequest(Guid.NewGuid(), "Rate B", Guid.NewGuid());
        var rateResponse = new RateResponse
        {
            Id = Guid.NewGuid(),
            Name = "Hotel B",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid()
        };
        var result = Result<RateResponse>.Success(rateResponse);

        _mediatorMock
            .Setup(m => m.Send(updateRateRequest, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Update(updateRateRequest, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<RateResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(rateResponse);
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsOkResultWithRateResponse()
    {
        // Arrange
        var rateId = Guid.NewGuid();
        var rateResponse = new RateResponse
        {
            Id = rateId,
            Name = "Deleted Rate Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid()
        };
        var result = Result<RateResponse>.Success(rateResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteRateRequest>(), default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Delete(rateId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Result as OkObjectResult;
        var responseResult = okResult!.Value as Result<RateResponse>;
        responseResult.Should().NotBeNull();
        responseResult!.Data.Should().BeEquivalentTo(rateResponse);
    }
}
