﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;

public sealed class GetGuestByIdHandler : IRequestHandler<GetGuestByIdRequest, Result<GuestResponse>>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public GetGuestByIdHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _guestRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<GuestResponse>> Handle(GetGuestByIdRequest request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.Get(request.Id);

        if (guest is null)
        {
            return Result<GuestResponse>.Failure(NotFoundError.NotFound($"Guest with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<GuestResponse>(guest);

        return Result<GuestResponse>.Success(response);
    }
}
