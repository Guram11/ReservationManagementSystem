using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;

public sealed record GetAllRateRoomTypesRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<Result<List<RateRoomTypeResponse>>>;
