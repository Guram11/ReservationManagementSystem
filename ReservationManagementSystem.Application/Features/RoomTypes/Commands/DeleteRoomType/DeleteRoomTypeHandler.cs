﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;

public sealed class DeleteRoomTypeHandler : IRequestHandler<DeleteRoomTypeRequest, Result<RoomTypeResponse>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public DeleteRoomTypeHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<RoomTypeResponse>> Handle(DeleteRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _roomTypeRepository.Delete(request.Id);
        var response = _mapper.Map<RoomTypeResponse>(hotel);

        return Result<RoomTypeResponse>.Success(response);
    }
}

