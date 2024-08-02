﻿using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;

public sealed record GetAllUserRequest(string? FilterOn, string? FilterQuery,
        string? SortBy, bool IsAscending,
        int PageNumber, int PageSize) : IRequest<List<GuestResponse>>;