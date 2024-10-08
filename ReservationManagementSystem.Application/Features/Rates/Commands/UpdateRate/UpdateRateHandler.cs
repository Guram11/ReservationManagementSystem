﻿using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;

public sealed class UpdateRateHandler : IRequestHandler<UpdateRateRequest, Result<RateResponse>>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateRateRequest> _validator;

    public UpdateRateHandler(IRateRepository rateRepository, IMapper mapper, IValidator<UpdateRateRequest> validator)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<RateResponse>> Handle(UpdateRateRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<RateResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var rate = _mapper.Map<Rate>(request);
        await _rateRepository.Update(request.Id, rate, cancellationToken);
        var response = _mapper.Map<RateResponse>(rate);

        return Result<RateResponse>.Success(response);
    }
}
