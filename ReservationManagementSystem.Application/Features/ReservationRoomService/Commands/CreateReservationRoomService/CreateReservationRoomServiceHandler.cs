using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.CreateReservationRoomService;

public sealed class CreateReservationRoomServiceHandler : IRequestHandler<CreateReservationRoomServiceRequest, Result<ReservationRoomServiceResponse>>
{
    private readonly IReservationRoomServiceRepository _reservationRoomServiceRepository;
    private readonly IMapper _mapper;

    public CreateReservationRoomServiceHandler(IReservationRoomServiceRepository reservationRoomServiceRepository, IMapper mapper)
    {
        _reservationRoomServiceRepository = reservationRoomServiceRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomServiceResponse>> Handle(CreateReservationRoomServiceRequest request, CancellationToken cancellationToken)
    {
        var reservationRoomService = _mapper.Map<ReservationRoomServices>(request);
        var createdService = await _reservationRoomServiceRepository.Create(reservationRoomService);

        if (createdService == null)
        {
            return Result<ReservationRoomServiceResponse>.Failure(ReservationRoomServiceErrors.NotFound());
        }

        var response = _mapper.Map<ReservationRoomServiceResponse>(reservationRoomService);
        return Result<ReservationRoomServiceResponse>.Success(response);
    }
}
