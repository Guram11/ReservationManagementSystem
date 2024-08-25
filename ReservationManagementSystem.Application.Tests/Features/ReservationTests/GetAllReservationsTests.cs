using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Features.Reservations.Queries;
using ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Tests.Features.ReservationTests;

public class GetAllReservationsHandlerTests
{
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllReservationsHandler _handler;

    public GetAllReservationsHandlerTests()
    {
        _mockReservationRepository = new Mock<IReservationRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllReservationsHandler(_mockReservationRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfReservationResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllReservationsRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var reservations = new List<Reservation>
        {
            new Reservation
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.NewGuid(),
                StatusId = ReservationStatus.Reserved,
                Checkin = DateTime.UtcNow.AddDays(1),
                Checkout = DateTime.UtcNow.AddDays(3),
                Currency = Currencies.USD,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                HotelId = Guid.NewGuid(),
                StatusId = ReservationStatus.Created,
                Checkin = DateTime.UtcNow.AddDays(5),
                Checkout = DateTime.UtcNow.AddDays(7),
                Currency = Currencies.EUR,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        var reservationResponses = new List<ReservationResponse>
        {
            new ReservationResponse
            {
                Id = reservations[0].Id,
                HotelId = reservations[0].HotelId,
                StatusId = reservations[0].StatusId,
                Checkin = reservations[0].Checkin,
                Checkout = reservations[0].Checkout,
                Currency = reservations[0].Currency,
                CreatedAt = reservations[0].CreatedAt,
                UpdatedAt = reservations[0].UpdatedAt
            },
            new ReservationResponse
            {
                Id = reservations[1].Id,
                HotelId = reservations[1].HotelId,
                StatusId = reservations[1].StatusId,
                Checkin = reservations[1].Checkin,
                Checkout = reservations[1].Checkout,
                Currency = reservations[1].Currency,
                CreatedAt = reservations[1].CreatedAt,
                UpdatedAt = reservations[1].UpdatedAt
            }
        };

        _mockReservationRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize
            ))
            .ReturnsAsync(reservations);

        _mockMapper.Setup(m => m.Map<List<ReservationResponse>>(reservations)).Returns(reservationResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(reservationResponses);
    }
}
