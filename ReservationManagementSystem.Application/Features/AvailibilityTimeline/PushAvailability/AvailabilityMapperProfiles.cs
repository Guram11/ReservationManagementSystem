using AutoMapper;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;

public class AvailabilityMapperProfiles : Profile
{
    public AvailabilityMapperProfiles()
    {
        CreateMap<AvailabilityTimeline, AvailabilityResponse>();
        CreateMap<PushAvailabilityRequest, AvailabilityTimeline>();
    }
}
