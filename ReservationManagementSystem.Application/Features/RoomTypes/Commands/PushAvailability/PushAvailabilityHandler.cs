using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.PushAvailability;

public sealed class PushAvailabilityHandler : IRequestHandler<PushAvailabilityRequest, Result<string>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public PushAvailabilityHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(PushAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetRoomTypeWithAvailabilityAsync(request.RoomTypeId);

        if (roomType is null)
        {
            return Result<string>.Failure(new Error("Not found.", "Room type not found."));
        }

        if (request.AvailableRooms > roomType.NumberOfRooms)
        {
            return Result<string>.Failure(new Error("Exceeding total number",$"Requested available rooms exceed the total number of rooms ({roomType.NumberOfRooms})."));
        }

        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            var availabilityTimeline = roomType.AvailabilityTimelines
                .FirstOrDefault(at => at.Date == date);

            if (availabilityTimeline == null)
            {
                availabilityTimeline = new AvailabilityTimeline
                {
                    Date = date,
                    RoomTypeId = request.RoomTypeId,
                    Available = request.AvailableRooms
                };
                roomType.AvailabilityTimelines.Add(availabilityTimeline);
            }
            else
            {
                availabilityTimeline.Available = request.AvailableRooms;
            }
        }

        await _roomTypeRepository.SaveChangesAsync();

        return Result<string>.Success("Availability updated successfully.");
    }
}
