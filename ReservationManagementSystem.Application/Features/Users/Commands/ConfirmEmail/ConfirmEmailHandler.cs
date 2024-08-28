using MediatR;
using ReservationManagementSystem.Application.Features.Users.Commands.Common;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest, Result<string>>
{
    private readonly IAccountService _accountService;

    public ConfirmEmailHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var response = await _accountService.ConfirmEmailAsync(request);

        if (response.IsSuccess)
        {
            return Result<string>.Success(SuccessResponses.EmailConfirmed);
        }

        return Result<string>.Failure(response.Error);
    }
}
