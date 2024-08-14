using MediatR;
using Microsoft.Extensions.Logging;
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
        var availabilityTimelines = await _availibilityTimelineRepository.GetAvailabilityByDateRange(request.DateFrom.Date, request.DateTo.Date);
        var rateTimelines = await _rateTimelineRepository.GetRatesByDateRange(request.DateFrom.Date, request.DateTo.Date);

        var results = new List<CheckAvailabilityResponse>();

        DateTime startDate = request.DateFrom.Date;
        DateTime endDate = request.DateTo.Date;

        var availableRoomTypeIds = availabilityTimelines.Select(at => at.RoomTypeId).Distinct();
        var rateRoomTypeIds = rateTimelines.Select(rt => rt.RoomTypeId).Distinct();
        var commonRoomTypeIds = availableRoomTypeIds.Intersect(rateRoomTypeIds).ToList();

        foreach (var roomType in roomTypes.Where(rt => commonRoomTypeIds.Contains(rt.Id)))
        {
            var roomAvailabilityTimelines = availabilityTimelines
                .Where(at => at.RoomTypeId == roomType.Id && at.Date.Date >= startDate && at.Date.Date <= endDate)
                .ToList();

            var roomRateTimelines = rateTimelines
                .Where(rt => rt.RoomTypeId == roomType.Id && rt.Date.Date >= startDate && rt.Date.Date <= endDate && rt.Price > 0)
                .ToList();

            var missingAvailabilityDates = new List<DateTime>();
            var missingRateDates = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!roomAvailabilityTimelines.Any(at => at.Date.Date == date.Date))
                {
                    Console.WriteLine($"Missing availability for {date.ToShortDateString()} in RoomType {roomType.Id}");
                    missingAvailabilityDates.Add(date);
                }

                if (!roomRateTimelines.Any(rt => rt.Date.Date == date.Date))
                {
                    Console.WriteLine($"Missing rate for {date.ToShortDateString()} in RoomType {roomType.Id}");
                    missingRateDates.Add(date);
                }
            }

            if (missingAvailabilityDates.Count == 0 && missingRateDates.Count == 0)
            {
                var availableRoom = roomAvailabilityTimelines.MinBy(at => at.Available);
                var validRateTimeline = roomRateTimelines.OrderBy(rt => rt.Date).FirstOrDefault();

                if (availableRoom?.Available > 0 && validRateTimeline != null)
                {
                    var totalPrice = roomRateTimelines.Sum(rt => rt.Price);
                    var rate = await _rateRepository.Get(validRateTimeline.RateId);

                    if (rate != null)
                    {
                        results.Add(new CheckAvailabilityResponse
                        {
                            RoomType = $"{roomType.Name}-{rate.Name}",
                            AvailableRooms = (int)(availableRoom.Available),
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

                Console.WriteLine($"Missing dates for RoomType {roomType.Id}: {string.Join(", ", missingDates.Select(d => d.ToShortDateString()))}");

                return Result<List<CheckAvailabilityResponse>>.Failure(new Error("Error", $"Availability or rate data missing for dates: {string.Join(", ", missingDates.Select(d => d.ToShortDateString()))}"));
            }
        }

        return Result<List<CheckAvailabilityResponse>>.Success(results);
    }
}
