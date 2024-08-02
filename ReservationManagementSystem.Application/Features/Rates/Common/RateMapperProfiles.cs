using AutoMapper;
using ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;
using ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rates.Common;

public class RateMapperProfiles : Profile
{
    public RateMapperProfiles()
    {
        CreateMap<Rate, RateResponse>();
        CreateMap<CreateRateRequest, Rate>();
        CreateMap<UpdateRateRequest, Rate>();
    }
}
