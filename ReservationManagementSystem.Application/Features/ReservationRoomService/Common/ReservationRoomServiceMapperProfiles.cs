using AutoMapper;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.CreateReservationRoomService;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

public class ReservationRoomServiceMapperProfiles : Profile
{
    public ReservationRoomServiceMapperProfiles()
    {
        CreateMap<ReservationRoomServices, ReservationRoomServiceResponse>();
        CreateMap<CreateReservationRoomServiceRequest, ReservationRoomServices>();
    }
}
