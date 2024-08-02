using AutoMapper;
using ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rooms.Common;

public class RoomsMapperProfiles : Profile
{
    public RoomsMapperProfiles()
    {
        CreateMap<Room, RoomResponse>();
        CreateMap<CreateRoomRequest, Room>();
        CreateMap<UpdateRoomRequest, Room>();
    }
}
