using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed class CreateReservationHandler : IRequestHandler<CreateReservationRequest, Result<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationRequest> _validator;
    private readonly IRequestHandler<CheckAvailabilityRequest, Result<List<CheckAvailabilityResponse>>> _checkAvailabilityHandler;

    public CreateReservationHandler(IReservationRepository reservationRepository, IMapper mapper,
        IValidator<CreateReservationRequest> validator,
        IRequestHandler<CheckAvailabilityRequest, Result<List<CheckAvailabilityResponse>>> checkAvailabilityHandler)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validator = validator;
        _checkAvailabilityHandler = checkAvailabilityHandler;
    }

    public async Task<Result<ReservationResponse>> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<ReservationResponse>.Failure(ValidationError.ValidationFailed(string.Join(", ", error)));
            }
        }

        var checkAvailabilityRequest = new CheckAvailabilityRequest(request.Checkin, request.Checkout);
        var availabilityResult = await _checkAvailabilityHandler.Handle(checkAvailabilityRequest, cancellationToken);

        if (!availabilityResult.IsSuccess)
        {
            return Result<ReservationResponse>.Failure(ReservationErrors.InvalidDataPassed());
        }

        var validResponse = availabilityResult.Data.FirstOrDefault(r =>
            r.RoomTypeId == request.RoomTypeId &&
            r.RateId == request.RateId &&
            request.NumberOfRooms <= r.AvailableRooms);

        if (validResponse == null)
        {
            return Result<ReservationResponse>.Failure(ReservationErrors.InvalidDataPassed());
        }

        var reservation = _mapper.Map<Reservation>(request);
        await _reservationRepository.Create(reservation, cancellationToken);

        var reservationResponse = _mapper.Map<ReservationResponse>(reservation);
        return Result<ReservationResponse>.Success(reservationResponse);
    }
}

