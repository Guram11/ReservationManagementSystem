using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;

public sealed class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeRequest, Result<RoomTypeResponse>>
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

    public async Task<Result<RoomTypeResponse>> Handle(UpdateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<RoomTypeResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var roomType = _mapper.Map<RoomType>(request);
        await _roomTypeRepository.Update(request.Id, roomType);
        var response = _mapper.Map<RoomTypeResponse>(roomType);

        return Result<RoomTypeResponse>.Success(response);
    }
}
