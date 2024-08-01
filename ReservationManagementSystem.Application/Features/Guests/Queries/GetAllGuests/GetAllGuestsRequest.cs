using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;

public sealed record GetAllUserRequest(string? FilterOn = null, string? FilterQuery = null,
        string? SortBy = null, bool IsAscending = true,
        int PageNumber = 1, int PageSize = 10) : IRequest<List<GuestResponse>>;