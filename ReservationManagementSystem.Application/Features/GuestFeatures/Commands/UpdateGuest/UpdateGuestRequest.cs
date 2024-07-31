using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Commands.UpdateGuest;

public sealed record UpdateGuestRequest(Guid Id, string Email, string FirstName, string LastName, string PhoneNumber) : IRequest<GuestResponse>;
