using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;

public sealed class GetRateByIdHandler : IRequestHandler<GetRateByIdRequest, RateResponse>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public GetRateByIdHandler(IRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }

    public async Task<RateResponse> Handle(GetRateByIdRequest request, CancellationToken cancellationToken)
    {
        var rate = await _rateRepository.Get(request.Id);
        return _mapper.Map<RateResponse>(rate);
    }
}
