﻿using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;

public sealed record GetAllReservationsRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<Result<List<ReservationResponse>>>;