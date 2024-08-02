using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;

public sealed record GetAllRoomTypesRequest(string? FilterOn = null, string? FilterQuery = null,
        string? SortBy = null, bool IsAscending = true,
        int PageNumber = 1, int PageSize = 10) : IRequest<List<RoomTypeResponse>>;
