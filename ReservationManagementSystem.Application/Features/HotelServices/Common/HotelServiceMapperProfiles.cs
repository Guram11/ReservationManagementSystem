using AutoMapper;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.HotelServices.Common;

public class HotelServiceMapperProfiles : Profile
{
    public HotelServiceMapperProfiles()
    {
        CreateMap<HotelService, HotelServiceResponse>();
        CreateMap<CreateHotelServiceRequest, HotelService>();
    }
}
