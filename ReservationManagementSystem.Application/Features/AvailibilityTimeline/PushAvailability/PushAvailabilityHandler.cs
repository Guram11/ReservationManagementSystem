using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;

public sealed class PushAvailabilityHandler : IRequestHandler<PushAvailabilityRequest, Result<AvailabilityResponse>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IAvailibilityTimelineRepository _availibilityTimelineRepository;
    private readonly IMapper _mapper;

    public PushAvailabilityHandler(IRoomTypeRepository roomTypeRepository,
        IAvailibilityTimelineRepository availibilityTimelineRepository, IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _availibilityTimelineRepository = availibilityTimelineRepository;
        _mapper = mapper;
    }

    public async Task<Result<AvailabilityResponse>> Handle(PushAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetRoomTypeWithAvailabilityAsync(request.RoomTypeId);

        if (roomType is null)
        {
            return Result<AvailabilityResponse>.Failure(new Error("Not found.", "Room type not found."));
        }

        if (request.AvailableRooms > roomType.NumberOfRooms)
        {
            return Result<AvailabilityResponse>.Failure(new Error("Exceeding total number", $"Requested available rooms exceed the total number of rooms ({roomType.NumberOfRooms})."));
        }

        var availabilityTimeline = new AvailabilityTimeline();

        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            availabilityTimeline = roomType?.AvailabilityTimelines?
               .FirstOrDefault(at => at.Date == date);

            if (availabilityTimeline == null)
            {
                availabilityTimeline = new AvailabilityTimeline
                {
                    Date = date,
                    RoomTypeId = request.RoomTypeId,
                    Available = request.AvailableRooms
                };
                await _availibilityTimelineRepository.Create(availabilityTimeline);
                roomType?.AvailabilityTimelines?.Add(availabilityTimeline);
            }
            else
            {
                availabilityTimeline.Available = request.AvailableRooms;
            }
        }

        await _roomTypeRepository.SaveChangesAsync();

        var respose = _mapper.Map<AvailabilityResponse>(availabilityTimeline);

        return Result<AvailabilityResponse>.Success(respose);
    }
}
