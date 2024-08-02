//using AutoMapper;
//using MediatR;
//using ReservationManagementSystem.Application.DTOs.Account;
//using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
//using ReservationManagementSystem.Application.Features.Guests.Common;
//using ReservationManagementSystem.Application.Interfaces.Repositories;
//using ReservationManagementSystem.Application.Interfaces.Services;
//using ReservationManagementSystem.Domain.Entities;

//namespace ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;

//public sealed class CreateUserHandler : IRequestHandler<UserRequest, UserResponse>
//{
//    private readonly IAccountService _accountService;
//    private readonly IMapper _mapper;

//    public CreateUserHandler(IAccountService accountService, IMapper mapper)
//    {
//        _accountService = accountService;
//        _mapper = mapper;
//    }

//    public async Task<GuestResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
//    {
//        var guest = _mapper.Map<Guest>(request);
//        await _accountService.RegisterAsync(request);

//        return _mapper.Map<GuestResponse>(guest);
//    }
//}
