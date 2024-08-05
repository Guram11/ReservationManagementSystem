using AutoMapper;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

public class RateRoomTypeMapperProfiles : Profile
{
    public RateRoomTypeMapperProfiles()
    {
        CreateMap<RateRoomType, RateRoomTypeResponse>();
        CreateMap<CreateRateRoomTypeRequest, RateRoomType>();
    }
}
