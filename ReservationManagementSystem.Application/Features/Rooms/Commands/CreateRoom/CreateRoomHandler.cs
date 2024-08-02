using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;

public sealed class CreateRoomHandler : IRequestHandler<CreateRoomRequest, RoomResponse>
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

    public async Task<RoomResponse> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var room = _mapper.Map<Room>(request);
        await _roomRepository.Create(room);

        return _mapper.Map<RoomResponse>(room);
    }
}

