using AutoMapper;
using ReservationManagementSystem.Application.Features.GuestFeatures.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.GuestFeatures.Commands.UpdateGuest;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Common;

public sealed class GuestMapperProfiles : Profile
{
    public GuestMapperProfiles()
    {
        CreateMap<Guest, GuestResponse>();
        CreateMap<CreateGuestRequest, Guest>();
        CreateMap<UpdateGuestRequest, Guest>();
    }
}
