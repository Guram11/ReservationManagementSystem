using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Commands.DeleteReservationRoom;

public sealed class DeleteReservationRoomHandler : IRequestHandler<DeleteReservationRoomRequest, Result<ReservationRoomResponse>>
{
    private readonly IReservationRoomRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public DeleteReservationRoomHandler(IReservationRoomRepository roomTypeRepository, IMapper mapper)
    {
        _reservationRoomRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomResponse>> Handle(DeleteReservationRoomRequest request, CancellationToken cancellationToken)
    {
        var isInUse = await _reservationRoomRepository.IsReservationRoomInUseAsync(request.ReservationRoomId);
        if (isInUse)
        {
            return Result<ReservationRoomResponse>.Failure(ValidationError.ResourceInUse());
        }

        var reservationRoom = await _reservationRoomRepository.Delete(request.ReservationRoomId, cancellationToken);

        if (reservationRoom == null)
        {
            return Result<ReservationRoomResponse>.Failure(ReservationRoomErrors.NotFound());
        }

        var response = _mapper.Map<ReservationRoomResponse>(reservationRoom);
        return Result<ReservationRoomResponse>.Success(response);
    }
}
