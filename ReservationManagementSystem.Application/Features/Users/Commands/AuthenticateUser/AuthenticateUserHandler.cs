//using AutoMapper;
//using MediatR;
//using ReservationManagementSystem.Application.DTOs.Account;
//using ReservationManagementSystem.Application.Interfaces.Services;

//namespace ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;

//public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserRequest, AuthenticationResponse>
//{
//    private readonly IAccountService _accountService;
//    private readonly IMapper _mapper;

//    public AuthenticateUserHandler(IAccountService accountService, IMapper mapper)
//    {
//        _accountService = accountService;
//        _mapper = mapper;
//    }

//    public async Task<AuthenticationResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
//    {
//        var user = _mapper.Map<AuthenticationResponse>(request);
//        await _accountService.AuthenticateAsync(request);

//        return _mapper.Map<AuthenticationResponse>(user);
//    }
//}