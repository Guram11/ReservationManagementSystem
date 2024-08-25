using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationTests;

public class DeleteReservationHandlerTests
{
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteReservationHandler _handler;

    public DeleteReservationHandlerTests()
    {
        _mockReservationRepository = new Mock<IReservationRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteReservationHandler(_mockReservationRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenReservationIsDeleted()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var reservation = new Reservation
        {
            Id = reservationId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            HotelId = Guid.NewGuid(),
            StatusId = ReservationStatus.Created,
            Checkin = DateTime.UtcNow.AddDays(1),
            Checkout = DateTime.UtcNow.AddDays(3),
            Currency = Currencies.USD
        };

        var reservationResponse = new ReservationResponse
        {
            Id = reservationId,
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt,
            HotelId = reservation.HotelId,
            StatusId = reservation.StatusId,
            Checkin = reservation.Checkin,
            Checkout = reservation.Checkout,
            Currency = reservation.Currency
        };

        _mockReservationRepository.Setup(repo => repo.Delete(reservationId)).ReturnsAsync(reservation);
        _mockMapper.Setup(m => m.Map<ReservationResponse>(reservation)).Returns(reservationResponse);

        var request = new DeleteReservationRequest(reservationId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(reservationResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReservationNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        _mockReservationRepository.Setup(repo => repo.Delete(reservationId)).ReturnsAsync((Reservation)null!);

        var request = new DeleteReservationRequest(reservationId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("NotFound");
        result.Error.Description.Should().Be($"Hotel service with ID {reservationId} was not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        _mockReservationRepository.Setup(repo => repo.Delete(reservationId)).ThrowsAsync(new Exception("Database error"));

        var request = new DeleteReservationRequest(reservationId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}