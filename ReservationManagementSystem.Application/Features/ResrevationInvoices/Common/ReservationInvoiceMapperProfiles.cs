using AutoMapper;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

public class ReservationInvoiceMapperProfiles : Profile
{
    public ReservationInvoiceMapperProfiles()
    {
        CreateMap<ReservationInvoices, ReservationInvoiceResponse>();
        CreateMap<CreateReservationRequest, ReservationInvoices>();
    }
}
