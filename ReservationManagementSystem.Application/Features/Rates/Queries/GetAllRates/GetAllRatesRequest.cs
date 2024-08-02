using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetAllRates;

public sealed record GetAllRatesRequest(string? FilterOn = null, string? FilterQuery = null,
        string? SortBy = null, bool IsAscending = true,
        int PageNumber = 1, int PageSize = 10) : IRequest<List<RateResponse>>;
