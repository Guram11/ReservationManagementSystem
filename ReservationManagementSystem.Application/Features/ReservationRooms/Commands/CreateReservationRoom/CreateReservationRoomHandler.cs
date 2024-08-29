using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;

public sealed class CreateReservationRoomlHandler : IRequestHandler<CreateReservationRoomRequest, Result<ReservationRoomResponse>>
{
    private readonly IReservationRoomRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public CreateReservationRoomlHandler(IReservationRoomRepository hotelServiceRepository, IMapper mapper)
    {
        _reservationRoomRepository = hotelServiceRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomResponse>> Handle(CreateReservationRoomRequest request, CancellationToken cancellationToken)
    {
        var reservationRoom = _mapper.Map<ReservationRoom>(request);
        var response =  await _reservationRoomRepository.Create(reservationRoom, cancellationToken);

        if (response is null)
        {
             return Result<ReservationRoomResponse>.Failure(ReservationRoomErrors.NotFound());
        }

        var reservationRoomResponse = _mapper.Map<ReservationRoomResponse>(reservationRoom);
        return Result<ReservationRoomResponse>.Success(reservationRoomResponse);
    }
}

