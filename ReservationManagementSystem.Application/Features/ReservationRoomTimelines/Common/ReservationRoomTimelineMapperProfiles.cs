using AutoMapper;
using ReservationManagementSystem.Application.Features.ReservationRoomTimelines.PushReservationTimeline;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;

public class ReservationRoomTimelineMapperProfiles : Profile
{
    public ReservationRoomTimelineMapperProfiles()
    {
        CreateMap<ReservationRoomTimeline, ReservationRoomTimelineResponse>();
        CreateMap<PushReservationTimelineRequest, ReservationRoomTimeline>();
    }
}
