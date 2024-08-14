using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;

public sealed record CreateHotelServiceRequest(Guid HotelId, HotelServiceTypes ServiceTypeId,
    string Description, decimal Price) : IRequest<Result<HotelServiceResponse>>;
