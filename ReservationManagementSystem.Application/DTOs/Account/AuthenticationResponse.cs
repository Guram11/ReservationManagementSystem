using System.Text.Json.Serialization;

namespace ReservationManagementSystem.Application.DTOs.Account;

public class AuthenticationResponse
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required List<string> Roles { get; set; }
    public bool IsVerified { get; set; }
    public required string JWToken { get; set; }
    [JsonIgnore]
    public string? RefreshToken { get; set; }
}
