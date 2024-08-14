using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Features.HotelServices.Queries;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.HotelServices;

public class GetAllHotelServicesHandlerTests
{
    private readonly Mock<IHotelServiceRepository> _mockHotelServiceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllHotelServicesHandler _handler;

    public GetAllHotelServicesHandlerTests()
    {
        _mockHotelServiceRepository = new Mock<IHotelServiceRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllHotelServicesHandler(_mockHotelServiceRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfHotelServiceResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllHotelServicesRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var hotelServices = new List<HotelService>
        {
            new HotelService { Id = Guid.NewGuid(), Description = "Service 1", Price = 100.00m, HotelId = Guid.NewGuid(),
                ServiceTypeId = HotelServiceTypes.MiniBar, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new HotelService { Id = Guid.NewGuid(), Description = "Service 2", Price = 200.00m, HotelId = Guid.NewGuid(),
                ServiceTypeId = HotelServiceTypes.HotelDamage, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        var hotelServiceResponses = new List<HotelServiceResponse>
        {
            new HotelServiceResponse { Id = hotelServices[0].Id, Description = hotelServices[0].Description,
                Price = hotelServices[0].Price, HotelId = hotelServices[0].HotelId,
                ServiceTypeId = hotelServices[0].ServiceTypeId, CreatedAt = hotelServices[0].CreatedAt,
                UpdatedAt = hotelServices[0].UpdatedAt },
            new HotelServiceResponse { Id = hotelServices[1].Id, Description = hotelServices[1].Description,
                Price = hotelServices[1].Price, HotelId = hotelServices[1].HotelId,
                ServiceTypeId = hotelServices[1].ServiceTypeId, CreatedAt = hotelServices[1].CreatedAt,
                UpdatedAt = hotelServices[1].UpdatedAt }
        };

        _mockHotelServiceRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize
            ))
            .ReturnsAsync(hotelServices);

        _mockMapper.Setup(m => m.Map<List<HotelServiceResponse>>(hotelServices)).Returns(hotelServiceResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(hotelServiceResponses);
    }
}
