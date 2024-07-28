using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.CreateGuest;

public sealed class CreateUserHandler : IRequestHandler<CreateGuestRequest, GuestResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUnitOfWork unitOfWork, IGuestRepository guestRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _guestRepository = guestRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = _mapper.Map<Guest>(request);
        _guestRepository.Create(guest);
        await _unitOfWork.Save(cancellationToken);

        return _mapper.Map<GuestResponse>(guest);
    }
}