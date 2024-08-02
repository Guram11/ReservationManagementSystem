using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;

public sealed class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeRequest, RoomTypeResponse>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateRoomTypeRequest> _validator;

    public UpdateRoomTypeHandler(IRoomTypeRepository roomRepository, IMapper mapper, IValidator<UpdateRoomTypeRequest> validator)
    {
        _roomTypeRepository = roomRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<RoomTypeResponse> Handle(UpdateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var roomType = _mapper.Map<RoomType>(request);
        await _roomTypeRepository.Update(request.Id, roomType);

        return _mapper.Map<RoomTypeResponse>(roomType);
    }
}
