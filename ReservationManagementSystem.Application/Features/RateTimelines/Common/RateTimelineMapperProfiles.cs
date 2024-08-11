using AutoMapper;
using ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RateTimelines.Common;

public class RateTimelineMapperProfiles : Profile
{
    public RateTimelineMapperProfiles()
    {
        CreateMap<RateTimeline, RateTimelineResponse>();
        CreateMap<PushPriceRequest, RateTimeline>();
    }
}
