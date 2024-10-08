﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
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
        var roomType = await _roomTypeRepository.GetRoomTypeWithAvailabilityAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
        {
            return Result<AvailabilityResponse>.Failure(RoomTypeErrors.NotFound(request.RoomTypeId));
        }

        if (request.AvailableRooms > roomType.NumberOfRooms)
        {
            return Result<AvailabilityResponse>.Failure(AvailibilityErrors.ExceedingNumberOfRooms(roomType.NumberOfRooms));
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
                await _availibilityTimelineRepository.Create(availabilityTimeline, cancellationToken);
                roomType?.AvailabilityTimelines?.Add(availabilityTimeline);
            }
            else
            {
                availabilityTimeline.Available = request.AvailableRooms;
            }
        }

        await _roomTypeRepository.SaveChangesAsync(cancellationToken);

        var respose = _mapper.Map<AvailabilityResponse>(availabilityTimeline);

        return Result<AvailabilityResponse>.Success(respose);
    }
}
