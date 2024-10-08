﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Queries.GetReservationById;

public sealed class GetReservationByIdHandler : IRequestHandler<GetReservationByIdRequest, Result<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public GetReservationByIdHandler(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationResponse>> Handle(GetReservationByIdRequest request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.Get(request.Id, cancellationToken);

        if (reservation is null)
        {
            return Result<ReservationResponse>.Failure(ReservationErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<ReservationResponse>(reservation);
        return Result<ReservationResponse>.Success(response);
    }
}
