using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public sealed class GetRoomTypeByIdHandler : IRequestHandler<GetRoomTypeByIdRequest, Result<RoomTypeResponse>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public GetRoomTypeByIdHandler(IRoomTypeRepository roomRepository, IMapper mapper)
    {
        _roomTypeRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<RoomTypeResponse>> Handle(GetRoomTypeByIdRequest request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetRoomTypeWithAvailabilityAsync(request.Id);

        if (roomType is null)
        {
            return Result<RoomTypeResponse>.Failure(NotFoundError.NotFound($"RoomType with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<RoomTypeResponse>(roomType);
        return Result<RoomTypeResponse>.Success(response);
    }
}