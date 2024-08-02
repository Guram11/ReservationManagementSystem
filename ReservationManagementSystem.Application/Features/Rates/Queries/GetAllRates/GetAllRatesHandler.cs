using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetAllRates;

public sealed class GetAllRatesHandler : IRequestHandler<GetAllRatesRequest, List<RateResponse>>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public GetAllRatesHandler(IRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }

    public async Task<List<RateResponse>> Handle(GetAllRatesRequest request, CancellationToken cancellationToken)
    {
        var rates = await _rateRepository.GetAll(cancellationToken,
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<RateResponse>>(rates);
    }
}
