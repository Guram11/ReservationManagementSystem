using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Queries.GetAllReservationRoomServices;


public sealed record GetAllReservationRoomServicesRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<ReservationRoomServiceResponse>>;
