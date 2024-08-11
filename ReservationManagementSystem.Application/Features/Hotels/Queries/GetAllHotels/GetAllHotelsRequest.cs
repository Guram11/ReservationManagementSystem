using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;

public sealed record GetAllHotelsRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<HotelResponse>>;
