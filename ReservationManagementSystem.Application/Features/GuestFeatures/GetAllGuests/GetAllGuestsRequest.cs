using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.GetAllGuests;

public sealed record GetAllUserRequest : IRequest<List<GuestResponse>>;