using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;

public sealed class CreateRoomHandler : IRequestHandler<CreateRoomRequest, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRoomRequest> _validator;

    public CreateRoomHandler(IRoomRepository roomRepository, IMapper mapper, IValidator<CreateRoomRequest> validator)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<RoomResponse>> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<RoomResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var room = _mapper.Map<Room>(request);
        await _roomRepository.Create(room);

        var roomResponse = _mapper.Map<RoomResponse>(room);
        return Result<RoomResponse>.Success(roomResponse);
    }
}

