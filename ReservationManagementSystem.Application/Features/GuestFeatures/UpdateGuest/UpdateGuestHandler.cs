using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.UpdateGuest;

public sealed class UpdateGuestHandler : IRequestHandler<UpdateGuestRequest, GuestResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public UpdateGuestHandler(IUnitOfWork unitOfWork, IGuestRepository guestRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _guestRepository = guestRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(UpdateGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Update(request.Id, guest);
        await _unitOfWork.Save(cancellationToken);

        return _mapper.Map<GuestResponse>(guest);
    }
}
