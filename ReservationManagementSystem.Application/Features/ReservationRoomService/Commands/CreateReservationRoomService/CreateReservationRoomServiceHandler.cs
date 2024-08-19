using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
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
        var createdRateRoomType = await _reservationRoomServiceRepository.Create(reservationRoomService);

        if (createdRateRoomType == null)
        {
            return Result<ReservationRoomServiceResponse>.Failure(NotFoundError.NotFound("RateRoomType was not found!"));
        }

        var response = _mapper.Map<ReservationRoomServiceResponse>(reservationRoomService);
        return Result<ReservationRoomServiceResponse>.Success(response);
    }
}
