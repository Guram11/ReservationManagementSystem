using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed record GetHotelByIdRequest(Guid Id) : IRequest<HotelResponse>;
