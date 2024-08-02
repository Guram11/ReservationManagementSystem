using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;

public sealed record GetAllRoomsRequest(string? FilterOn = null, string? FilterQuery = null,
        string? SortBy = null, bool IsAscending = true,
        int PageNumber = 1, int PageSize = 10) : IRequest<List<RoomResponse>>;
