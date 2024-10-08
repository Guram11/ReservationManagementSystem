﻿using MediatR;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;

public sealed record CreateReservationInvoiceRequest(Guid ReservationId, decimal Paid,
    Currencies Currency) : IRequest<Result<ReservationInvoiceResponse>>;
