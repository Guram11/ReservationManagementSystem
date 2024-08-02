using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;

public sealed class DeleteRateHandler : IRequestHandler<DeleteRateRequest, RateResponse>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public DeleteRateHandler(IRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }

    public async Task<RateResponse> Handle(DeleteRateRequest request, CancellationToken cancellationToken)
    {
        var rate = await _rateRepository.Delete(request.Id);

        return _mapper.Map<RateResponse>(rate);
    }
}
