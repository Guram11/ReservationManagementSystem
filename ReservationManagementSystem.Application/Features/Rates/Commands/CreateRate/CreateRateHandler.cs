using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;

public sealed class CreateRateHandler : IRequestHandler<CreateRateRequest, RateResponse>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRateRequest> _validator;

    public CreateRateHandler(IRateRepository rateRepository, IMapper mapper, IValidator<CreateRateRequest> validator)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<RateResponse> Handle(CreateRateRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var rate = _mapper.Map<Rate>(request);
        await _rateRepository.Create(rate);

        return _mapper.Map<RateResponse>(rate);
    }
}
