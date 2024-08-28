using AutoMapper;
using FluentAssertions;
using Moq;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Tests.Features.Guests;

public class GetAllUserHandlerTests
{
    private readonly Mock<IGuestRepository> _mockGuestRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllUserHandler _handler;

    public GetAllUserHandlerTests()
    {
        _mockGuestRepository = new Mock<IGuestRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllUserHandler(_mockGuestRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfGuestResponses_WhenCalled()
    {
        // Arrange
        var request = new GetAllGuestsRequest(
            FilterOn: null,
            FilterQuery: null,
            SortBy: null,
            IsAscending: true,
            PageNumber: 1,
            PageSize: 10
        );

        var guests = new List<Guest>
        {
            new Guest { Id = Guid.NewGuid(), Email = "guest1@example.com", FirstName = "John", LastName = "Doe", PhoneNumber = "1234567890", ReservationRoomId = Guid.NewGuid() },
            new Guest { Id = Guid.NewGuid(), Email = "guest2@example.com", FirstName = "Jane", LastName = "Smith", PhoneNumber = "0987654321", ReservationRoomId = Guid.NewGuid() }
        };

        var guestResponses = new List<GuestResponse>
        {
            new GuestResponse { Id = guests[0].Id, Email = guests[0].Email, FirstName = guests[0].FirstName, LastName = guests[0].LastName, PhoneNumber = guests[0].PhoneNumber, ReservationRoomId = guests[0].ReservationRoomId },
            new GuestResponse { Id = guests[1].Id, Email = guests[1].Email, FirstName = guests[1].FirstName, LastName = guests[1].LastName, PhoneNumber = guests[1].PhoneNumber, ReservationRoomId = guests[1].ReservationRoomId }
        };

        _mockGuestRepository.Setup(repo => repo.GetAll(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending,
                request.PageNumber,
                request.PageSize
            ))
            .ReturnsAsync(guests);

        _mockMapper.Setup(m => m.Map<List<GuestResponse>>(guests)).Returns(guestResponses);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(guestResponses);
    }
}
