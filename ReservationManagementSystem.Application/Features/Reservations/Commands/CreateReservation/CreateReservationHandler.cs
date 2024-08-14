using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed class CreateReservationlHandler : IRequestHandler<CreateReservationRequest, Result<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationRequest> _validator;

    public CreateReservationlHandler(IReservationRepository reservationRepository, IMapper mapper, IValidator<CreateReservationRequest> validator)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<ReservationResponse>> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<ReservationResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var reservation = _mapper.Map<Reservation>(request);
        await _reservationRepository.Create(reservation);

        var reservationResponse = _mapper.Map<ReservationResponse>(reservation);
        return Result<ReservationResponse>.Success(reservationResponse);
    }
}

