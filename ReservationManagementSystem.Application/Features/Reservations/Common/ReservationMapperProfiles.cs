using AutoMapper;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public class ReservationMapperProfiles : Profile
{
    public ReservationMapperProfiles()
    {
        CreateMap<Reservation, ReservationResponse>();
        CreateMap<CreateReservationRequest, Reservation>();
    }
}

