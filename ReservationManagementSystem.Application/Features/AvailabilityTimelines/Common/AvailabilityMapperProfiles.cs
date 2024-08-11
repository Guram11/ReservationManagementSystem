using AutoMapper;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.PushAvailability;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;

public class AvailabilityMapperProfiles : Profile
{
    public AvailabilityMapperProfiles()
    {
        CreateMap<AvailabilityTimeline, AvailabilityResponse>();
        CreateMap<PushAvailabilityRequest, AvailabilityTimeline>();
    }
}
