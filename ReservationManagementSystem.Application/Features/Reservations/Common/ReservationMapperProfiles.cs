using AutoMapper;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public class ReservationMapperProfiles : Profile
{
    public ReservationMapperProfiles()
    {
        CreateMap<ReservationInvoices, ReservationInvoiceResponse>();
        CreateMap<CreateReservationInvoiceRequest, ReservationInvoices>();
    }
}

