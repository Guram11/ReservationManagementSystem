using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed record GetHotelByIdRequest(Guid Id) : IRequest<Result<HotelResponse>>;
