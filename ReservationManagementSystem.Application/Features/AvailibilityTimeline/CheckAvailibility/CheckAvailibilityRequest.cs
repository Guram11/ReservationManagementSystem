using MediatR;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

public sealed record CheckAvailabilityRequest(DateTime DateFrom, DateTime DateTo) : IRequest<Result<List<CheckAvailabilityResponse>>>;
