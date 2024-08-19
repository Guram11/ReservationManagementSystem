using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.DeleteReservationRoomService;

public sealed class DeleteReservationRoomServiceHandler : IRequestHandler<DeleteReservationRoomServiceRequest, Result<ReservationRoomServiceResponse>>
{
    private readonly IReservationRoomServiceRepository _reservationRoomServiceRepository;
    private readonly IMapper _mapper;

    public DeleteReservationRoomServiceHandler(IReservationRoomServiceRepository roomTypeRepository, IMapper mapper)
    {
        _reservationRoomServiceRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomServiceResponse>> Handle(DeleteReservationRoomServiceRequest request, CancellationToken cancellationToken)
    {
        var reservationRoomService = await _reservationRoomServiceRepository.Delete(request.ReservationRoomId, request.HotelServiceId);

        if (reservationRoomService == null)
        {
            return Result<ReservationRoomServiceResponse>.Failure(NotFoundError.NotFound("RateRoomType was not found!"));
        }

        var response = _mapper.Map<ReservationRoomServiceResponse>(reservationRoomService);
        return Result<ReservationRoomServiceResponse>.Success(response);
    }
}
