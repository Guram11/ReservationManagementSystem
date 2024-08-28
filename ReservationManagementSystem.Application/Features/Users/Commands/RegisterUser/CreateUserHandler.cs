using MediatR;
using ReservationManagementSystem.Application.Features.Users.Commands.Common;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;

public sealed class CreateUserHandler : IRequestHandler<CreateUserRequest, Result<string>>
{
    private readonly IAccountService _accountService;

    public CreateUserHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _accountService.RegisterAsync(request);

        if (response.IsSuccess)
        {
            return Result<string>.Success(SuccessResponses.VerificationEmailSent);
        }

        return Result<string>.Failure(response.Error);
    }
}
