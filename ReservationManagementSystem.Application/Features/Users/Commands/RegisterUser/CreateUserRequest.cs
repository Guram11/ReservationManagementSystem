using MediatR;
using ReservationManagementSystem.Application.Wrappers;
using System.Text.Json.Serialization;

namespace ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;
public class CreateUserRequest : IRequest<Result<string>>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirm { get; set; }
    [JsonIgnore]
    public string? Origin { get; set; }
}