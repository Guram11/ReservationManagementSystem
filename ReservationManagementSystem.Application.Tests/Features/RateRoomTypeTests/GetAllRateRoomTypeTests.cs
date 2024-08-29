using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.RateRoomTypeTests;

public class GetAllRateRoomTypesHandlerTests
{
    private readonly Mock<IRateRoomTypeRepository> _rateRoomTypeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllRateRoomTypesHandler _handler;

    public GetAllRateRoomTypesHandlerTests()
    {
        _rateRoomTypeRepositoryMock = new Mock<IRateRoomTypeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllRateRoomTypesHandler(_rateRoomTypeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResultWithRoomTypes()
    {
        // Arrange
        var roomTypes = new List<RateRoomType>
        {
            new RateRoomType { RateId = Guid.NewGuid(), RoomTypeId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new RateRoomType { RateId = Guid.NewGuid(), RoomTypeId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        var request = new GetAllRateRoomTypesRequest(null, null, null, true, 1, 10);

        _rateRoomTypeRepositoryMock.Setup(x => x.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, CancellationToken.None))
            .ReturnsAsync(roomTypes);

        var rateRoomTypeResponses = new List<RateRoomTypeResponse>
        {
            new RateRoomTypeResponse { RateId = roomTypes[0].RateId, RoomTypeId = roomTypes[0].RoomTypeId, CreatedAt = roomTypes[0].CreatedAt, UpdatedAt = roomTypes[0].UpdatedAt },
            new RateRoomTypeResponse { RateId = roomTypes[1].RateId, RoomTypeId = roomTypes[1].RoomTypeId, CreatedAt = roomTypes[1].CreatedAt, UpdatedAt = roomTypes[1].UpdatedAt }
        };

        _mapperMock.Setup(x => x.Map<List<RateRoomTypeResponse>>(roomTypes))
            .Returns(rateRoomTypeResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(rateRoomTypeResponses);
    }

    [Fact]
    public async Task Handle_EmptyResult_ReturnsSuccessResultWithEmptyList()
    {
        // Arrange
        var roomTypes = new List<RateRoomType>();

        var request = new GetAllRateRoomTypesRequest(null, null, null, true, 1, 10);

        _rateRoomTypeRepositoryMock.Setup(x => x.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, CancellationToken.None))
            .ReturnsAsync(roomTypes);

        var rateRoomTypeResponses = new List<RateRoomTypeResponse>();

        _mapperMock.Setup(x => x.Map<List<RateRoomTypeResponse>>(roomTypes))
            .Returns(rateRoomTypeResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
    }
}
