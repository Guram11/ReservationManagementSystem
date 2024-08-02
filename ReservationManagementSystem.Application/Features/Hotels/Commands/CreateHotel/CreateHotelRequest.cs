using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed record CreateHotelRequest(string Name) : IRequest<HotelResponse>;