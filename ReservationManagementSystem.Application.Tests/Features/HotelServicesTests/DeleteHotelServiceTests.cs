using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.HotelServices;

public class DeleteHotelServiceTests
{
    private readonly Mock<IHotelServiceRepository> _mockHotelServiceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteHoteServicelHandler _handler;

    public DeleteHotelServiceTests()
    {
        _mockHotelServiceRepository = new Mock<IHotelServiceRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteHoteServicelHandler(_mockHotelServiceRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelServiceIsDeleted()
    {
        // Arrange
        var hotelServiceId = Guid.NewGuid();
        var hotelService = new HotelService
        {
            Id = hotelServiceId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            ServiceTypeId = HotelServiceTypes.MiniBar,
            Description = "Spa service",
            Price = 100.00m
        };

        var hotelServiceResponse = new HotelServiceResponse
        {
            Id = hotelServiceId,
            CreatedAt = hotelService.CreatedAt,
            UpdatedAt = hotelService.UpdatedAt,
            HotelId = hotelService.HotelId,
            ServiceTypeId = hotelService.ServiceTypeId,
            Description = hotelService.Description,
            Price = hotelService.Price
        };

        _mockHotelServiceRepository.Setup(repo => repo.Delete(hotelServiceId, CancellationToken.None)).ReturnsAsync(hotelService);
        _mockMapper.Setup(m => m.Map<HotelServiceResponse>(hotelService)).Returns(hotelServiceResponse);

        var request = new DeleteHotelServiceRequest(hotelServiceId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(hotelServiceResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHotelServiceNotFound()
    {
        // Arrange
        var hotelServiceId = Guid.NewGuid();
        _mockHotelServiceRepository.Setup(repo => repo.Delete(hotelServiceId, CancellationToken.None)).ReturnsAsync((HotelService)null!);

        var request = new DeleteHotelServiceRequest(hotelServiceId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ErrorType.Should().Be(Enums.ErrorType.NotFoundError);
        result.Error.Description.Should().Be($"Hotel service with ID {hotelServiceId} was not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var hotelServiceId = Guid.NewGuid();
        _mockHotelServiceRepository.Setup(repo => repo.Delete(hotelServiceId, CancellationToken.None)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteHotelServiceRequest(hotelServiceId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}

