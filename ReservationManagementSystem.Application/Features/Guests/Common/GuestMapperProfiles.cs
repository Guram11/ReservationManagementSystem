using AutoMapper;
using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Guests.Common;

public sealed class GuestMapperProfiles : Profile
{
    public GuestMapperProfiles()
    {
        CreateMap<Guest, GuestResponse>();
        CreateMap<CreateGuestRequest, Guest>();
        CreateMap<UpdateGuestRequest, Guest>();
    }
}
