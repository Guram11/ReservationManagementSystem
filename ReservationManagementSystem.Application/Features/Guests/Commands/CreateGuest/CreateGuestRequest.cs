using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;

public sealed record CreateGuestRequest(string Email, string FirstName, string LastName,
    string PhoneNumber, Guid ReservationRoomId) : IRequest<Result<GuestResponse>>;
