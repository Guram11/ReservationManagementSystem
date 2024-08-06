using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels.Commands.DeleteHotel;

public class DeleteHotelHandlerTests
{
    private readonly Mock<IHotelRepository> _mockHotelRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteHotelHandler _handler;

    public DeleteHotelHandlerTests()
    {
        _mockHotelRepository = new Mock<IHotelRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteHotelHandler(_mockHotelRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Hotel_Deleted()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { Id = hotelId, Name = "Test Hotel" };
        var hotelResponse = new HotelResponse { Id = hotelId, Name = hotel.Name };

        _mockHotelRepository.Setup(repo => repo.Delete(hotelId)).ReturnsAsync(hotel);
        _mockMapper.Setup(m => m.Map<HotelResponse>(hotel)).Returns(hotelResponse);

        var request = new DeleteHotelRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Hotel_Not_Found()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.Delete(hotelId)).ReturnsAsync((Hotel)null);

        var request = new DeleteHotelRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("HotelNotFound");
        result.Error.Description.Should().Be($"Hotel with ID {hotelId} was not found.");
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Repository_Throws()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.Delete(hotelId)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteHotelRequest(hotelId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
