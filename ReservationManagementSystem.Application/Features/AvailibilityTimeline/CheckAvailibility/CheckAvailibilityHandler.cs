using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

public sealed class CheckAvailabilityHandler : IRequestHandler<CheckAvailabilityRequest, Result<List<CheckAvailabilityResponse>>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IAvailibilityTimelineRepository _availibilityTimelineRepository;
    private readonly IRateTimelineRepository _rateTimelineRepository;
    private readonly IRateRepository _rateRepository;

    public CheckAvailabilityHandler(IRoomTypeRepository roomTypeRepository,
        IAvailibilityTimelineRepository availibilityTimelineRepository,
        IRateTimelineRepository rateTimelineRepository,
        IRateRepository rateRepository)
    {
        _roomTypeRepository = roomTypeRepository;
        _availibilityTimelineRepository = availibilityTimelineRepository;
        _rateTimelineRepository = rateTimelineRepository;
        _rateRepository = rateRepository;
    }

    public async Task<Result<List<CheckAvailabilityResponse>>> Handle(CheckAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var roomTypes = await _roomTypeRepository.GetAll(null, null, null, true, 1, 10);
        var availabilityTimelines = await _availibilityTimelineRepository.GetAvailabilityByDateRange(request.DateFrom, request.DateTo);
        var rateTimelines = await _rateTimelineRepository.GetRatesByDateRange(request.DateFrom, request.DateTo);

        var results = new List<CheckAvailabilityResponse>();

        DateTime startDate = request.DateFrom.Date;
        DateTime endDate = request.DateTo.Date.AddDays(1);

        var availableRoomTypeIds = availabilityTimelines.Select(at => at.RoomTypeId).Distinct();
        var rateRoomTypeIds = rateTimelines.Select(rt => rt.RoomTypeId).Distinct();
        var commonRoomTypeIds = availableRoomTypeIds.Intersect(rateRoomTypeIds).ToList();

        bool allOptionsHaveMissingDates = true;

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
                allOptionsHaveMissingDates = false;

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
        }

        if (results.Any())
        {
            return Result<List<CheckAvailabilityResponse>>.Success(results);
        }

        if (allOptionsHaveMissingDates)
        {
            return Result<List<CheckAvailabilityResponse>>.Failure(NoAvailableOptionsError.NoAvailableOptions());
        }

        return Result<List<CheckAvailabilityResponse>>.Success(new List<CheckAvailabilityResponse>());
    }
}