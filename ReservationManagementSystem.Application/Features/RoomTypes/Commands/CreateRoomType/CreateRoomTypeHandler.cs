using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;

public sealed class CreateRoomTypeHandler : IRequestHandler<CreateRoomTypeRequest, RoomTypeResponse>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRoomTypeRequest> _validator;

    public CreateRoomTypeHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper, IValidator<CreateRoomTypeRequest> validator)
    {
        _roomTypeRepository = roomTypeRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<RoomTypeResponse> Handle(CreateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var roomType = _mapper.Map<RoomType>(request);
        await _roomTypeRepository.Create(roomType);

        return _mapper.Map<RoomTypeResponse>(roomType);
    }
}
