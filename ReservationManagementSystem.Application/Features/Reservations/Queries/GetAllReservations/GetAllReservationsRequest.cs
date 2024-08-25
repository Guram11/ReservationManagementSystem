using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;

namespace ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;

public sealed record GetAllReservationsRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<ReservationResponse>>;
