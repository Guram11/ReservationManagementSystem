using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels.Queries.GetHotelById;

public class GetHotelByIdHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetHotelByIdHandler _handler;

    public GetHotelByIdHandlerTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetHotelByIdHandler(_mockHotelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Hotel_Is_Found()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { Id = hotelId, Name = "Sample Hotel" };
        var hotelResponse = new HotelResponse { Id = hotelId, Name = "Sample Hotel" };

        _mockHotelRepository.Setup(repo => repo.Get(hotelId))
            .ReturnsAsync(hotel);
        _mockMapper.Setup(m => m.Map<HotelResponse>(hotel))
            .Returns(hotelResponse);

        var request = new GetHotelByIdRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Hotel_Is_Not_Found()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.Get(hotelId))
            .ReturnsAsync((Hotel)null);

        var request = new GetHotelByIdRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("HotelNotFound");
        result.Error.Description.Should().Be($"Hotel with ID {hotelId} was not found.");
    }
}
