using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Queries.GetAllReservationRoomPayments;

public sealed record GetAllReservationRoomPaymentsRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<ReservationRoomPaymentsResponse>>;
