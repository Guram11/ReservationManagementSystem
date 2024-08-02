using AutoMapper;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Hotels.Common;

public class HotelMapperProfiles : Profile
{
    public HotelMapperProfiles() 
    {
        CreateMap<Hotel, HotelResponse>();
        CreateMap<CreateHotelRequest, Hotel>();
        CreateMap<UpdateHotelRequest, Hotel>();
    }
}
