using AutoMapper;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.CreateReservationRoomPayment;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;

public class ReservationRoomPaymentsMapperProfiles : Profile
{
    public ReservationRoomPaymentsMapperProfiles()
    {
        CreateMap<ReservationRoomPayments, ReservationRoomPaymentsResponse>();
        CreateMap<CreateReservationRoomPaymentRequest, ReservationRoomPayments>();
    }
}
