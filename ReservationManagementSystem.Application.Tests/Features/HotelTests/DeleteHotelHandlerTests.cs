using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels;

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
    public async Task Handle_ShouldReturnSuccess_WhenHotelIsDeleted()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { Id = hotelId, Name = "Test Hotel" };
        var hotelResponse = new HotelResponse { Id = hotelId, Name = hotel.Name };

        _mockHotelRepository.Setup(repo => repo.Delete(hotelId, CancellationToken.None)).ReturnsAsync(hotel);
        _mockMapper.Setup(m => m.Map<HotelResponse>(hotel)).Returns(hotelResponse);

        var request = new DeleteHotelRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHotelNotFound()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.Delete(hotelId, CancellationToken.None)).ReturnsAsync((Hotel)null!);

        var request = new DeleteHotelRequest(hotelId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.NotFoundError);
        result.Error.Description.Should().Be($"Hotel with ID {hotelId} was not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        _mockHotelRepository.Setup(repo => repo.Delete(hotelId, CancellationToken.None)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteHotelRequest(hotelId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
