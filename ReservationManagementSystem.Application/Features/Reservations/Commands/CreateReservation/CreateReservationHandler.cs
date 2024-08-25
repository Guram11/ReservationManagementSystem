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

public sealed class CreateReservationlHandler : IRequestHandler<CreateReservationRequest, Result<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationRequest> _validator;
    private readonly IAvailibilityTimelineRepository _availibilityTimelineRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IRateTimelineRepository _rateTimelineRepository;
    private readonly IRateRepository _rateRepository;

    public CreateReservationlHandler(IReservationRepository reservationRepository, IMapper mapper,
        IValidator<CreateReservationRequest> validator,
        IAvailibilityTimelineRepository availibilityTimelineRepository,
        IRoomTypeRepository roomTypeRepository,
        IRateTimelineRepository rateTimelineRepository,
        IRateRepository rateRepository)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validator = validator;
        _availibilityTimelineRepository = availibilityTimelineRepository;
        _roomTypeRepository = roomTypeRepository;
        _rateTimelineRepository = rateTimelineRepository;
        _rateRepository = rateRepository;
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

        var responses = await CheckAvailability(request.Checkin, request.Checkout);

        if (!IsValidReservation(request, responses))
        {
            return Result<ReservationResponse>.Failure(ValidationError.ValidationFailed("Invalid data passed, Please check room availibility"));
        }

        var reservation = _mapper.Map<Reservation>(request);
        await _reservationRepository.Create(reservation);

        var reservationResponse = _mapper.Map<ReservationResponse>(reservation);
        return Result<ReservationResponse>.Success(reservationResponse);
    }

    public static bool IsValidReservation(CreateReservationRequest request, List<CheckAvailabilityResponse> availabilityResponses)
    {
        int sum = 0;
        foreach (var response in availabilityResponses)
        {
            var responseRoomTypeId = response.RoomTypeId;
            var responseRateId = response.RateId;

            if (responseRoomTypeId == request.RoomTypeId && responseRateId == request.RateId && request.NumberOfRooms <= response.AvailableRooms)
            {
                sum++;
            }
        }
        return sum > 0;
    }

    public async Task<List<CheckAvailabilityResponse>> CheckAvailability(DateTime DateFrom, DateTime DateTo)
    {
        var roomTypes = await _roomTypeRepository.GetAll(null, null, null, true, 1, 10);
        var availabilityTimelines = await _availibilityTimelineRepository.GetAvailabilityByDateRange(DateFrom, DateTo);
        var rateTimelines = await _rateTimelineRepository.GetRatesByDateRange(DateFrom, DateTo);
        var results = new List<CheckAvailabilityResponse>();

        DateTime startDate = DateFrom.Date;
        DateTime endDate = DateTo.Date.AddDays(1);

        var availableRoomTypeIds = availabilityTimelines.Select(at => at.RoomTypeId).Distinct();
        var rateRoomTypeIds = rateTimelines.Select(rt => rt.RoomTypeId).Distinct();
        var commonRoomTypeIds = availableRoomTypeIds.Intersect(rateRoomTypeIds).ToList();

        foreach (var roomType in roomTypes.Where(rt => commonRoomTypeIds.Contains(rt.Id)))
        {
            var roomAvailabilityTimelines = availabilityTimelines
                .Where(at => at.RoomTypeId == roomType.Id && at.Date >= startDate && at.Date < endDate)
                .ToList();

            var roomRateTimelines = rateTimelines
                .Where(rt => rt.RoomTypeId == roomType.Id && rt.Date >= startDate && rt.Date < endDate && rt.Price > 0)
                .ToList();

            var missingAvailabilityDates = new List<DateTime>();
            var missingRateDates = new List<DateTime>();

            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                if (!roomAvailabilityTimelines.Any(at => at.Date.Date == date))
                {
                    missingAvailabilityDates.Add(date);
                }

                if (!roomRateTimelines.Any(rt => rt.Date.Date == date))
                {
                    missingRateDates.Add(date);
                }
            }

            if (missingAvailabilityDates.Count == 0 && missingRateDates.Count == 0)
            {
                var availableRoom = roomAvailabilityTimelines.OrderBy(at => at.Available).FirstOrDefault();
                var validRateTimeline = roomRateTimelines.OrderBy(rt => rt.Date).FirstOrDefault();

                if (availableRoom?.Available > 0 && validRateTimeline != null)
                {
                    var totalPrice = roomRateTimelines.Sum(rt => rt.Price);
                    var rate = await _rateRepository.Get(validRateTimeline.RateId);

                    if (rate != null)
                    {
                        results.Add(new CheckAvailabilityResponse
                        {
                            RateId = rate.Id,
                            RoomTypeId = roomType.Id,
                            RoomType = $"{roomType.Name}-{rate.Name}",
                            AvailableRooms = (int)availableRoom.Available,
                            TotalPrice = totalPrice
                        });
                    }
                }
            }
            else
            {
                var missingDates = missingAvailabilityDates.Concat(missingRateDates)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                return [];
            }
        }

        return results;
    }
}

