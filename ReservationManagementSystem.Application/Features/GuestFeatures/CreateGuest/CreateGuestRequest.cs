using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.CreateGuest;

public sealed record CreateGuestRequest(string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<GuestResponse>;
