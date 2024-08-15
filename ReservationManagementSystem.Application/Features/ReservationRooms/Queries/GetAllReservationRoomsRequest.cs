using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Queries;

public sealed record GetAllReservationRoomsRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<ReservationRoomResponse>>;
