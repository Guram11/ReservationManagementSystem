using AutoMapper;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Common;

public class RoomTypeMapperProfiles : Profile
{
    public RoomTypeMapperProfiles()
    {
        CreateMap<RoomType, RoomTypeResponse>();
        CreateMap<CreateRoomTypeRequest, RoomType>();
        CreateMap<UpdateRoomTypeRequest, RoomType>();
    }
}
