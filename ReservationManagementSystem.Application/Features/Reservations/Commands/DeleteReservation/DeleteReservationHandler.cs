﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;

public sealed class DeleteReservationHandler : IRequestHandler<DeleteReservationRequest, Result<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public DeleteReservationHandler(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationResponse>> Handle(DeleteReservationRequest request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.Delete(request.Id);

        if (reservation is null)
        {
            return Result<ReservationResponse>.Failure(NotFoundError.NotFound($"Hotel service with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<ReservationResponse>(reservation);

        return Result<ReservationResponse>.Success(response);
    }
}
