using AutoMapper;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Common;

public class ReservationRoomMapperProfiles : Profile
{
    public ReservationRoomMapperProfiles()
    {
        CreateMap<ReservationRoom, ReservationRoomResponse>();
        CreateMap<CreateReservationRoomRequest, ReservationRoom>();
    }
}