using AutoMapper;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

public class ReservationInvoiceMapperProfiles : Profile
{
    public ReservationInvoiceMapperProfiles()
    {
        CreateMap<ReservationInvoices, ReservationInvoiceResponse>();
        CreateMap<CreateReservationInvoiceRequest, ReservationInvoices>();
    }
}
