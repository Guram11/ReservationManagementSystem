using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;

namespace ReservationManagementSystem.Application.Features.HotelServices.Queries;

public sealed record GetAllHotelServicesRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<HotelServiceResponse>>;

