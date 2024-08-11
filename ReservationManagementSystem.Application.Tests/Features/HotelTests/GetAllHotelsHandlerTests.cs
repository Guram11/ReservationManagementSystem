using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels;

public class GetAllHotelsHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllHotelsHandler _handler;

    public GetAllHotelsHandlerTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllHotelsHandler(_mockHotelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfHotelResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllHotelsRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var hotels = new List<Hotel>
        {
            new Hotel { Id = Guid.NewGuid(), Name = "Hotel 1" },
            new Hotel { Id = Guid.NewGuid(), Name = "Hotel 2" }
        };

        var hotelResponses = new List<HotelResponse>
        {
            new HotelResponse { Id = hotels[0].Id, Name = hotels[0].Name },
            new HotelResponse { Id = hotels[1].Id, Name = hotels[1].Name }
        };

        _mockHotelRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize
            ))
            .ReturnsAsync(hotels);

        _mockMapper.Setup(m => m.Map<List<HotelResponse>>(hotels)).Returns(hotelResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(hotelResponses);
    }
}