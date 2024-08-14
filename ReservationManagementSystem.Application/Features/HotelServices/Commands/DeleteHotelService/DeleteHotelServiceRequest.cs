using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;

public sealed record DeleteHotelServiceRequest(Guid Id) : IRequest<Result<HotelServiceResponse>>;
