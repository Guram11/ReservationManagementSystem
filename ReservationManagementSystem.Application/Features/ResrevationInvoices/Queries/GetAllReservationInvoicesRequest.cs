using MediatR;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Queries;

public sealed record GetAllReservationInvoicesRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<ReservationInvoiceResponse>>;
