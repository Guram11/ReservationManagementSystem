using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;

public sealed record GetAllGuestsRequest(string? FilterOn = null, string? FilterQuery = null,
        string? SortBy = null, bool IsAscending = true,
        int PageNumber = 1, int PageSize = 10) : IRequest<List<HotelResponse>>;
