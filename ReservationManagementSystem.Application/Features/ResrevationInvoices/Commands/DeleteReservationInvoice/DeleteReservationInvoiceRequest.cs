using MediatR;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.DeleteReservationInvoice;

public sealed record DeleteReservationInvoiceRequest(Guid Id) : IRequest<Result<ReservationInvoiceResponse>>;
