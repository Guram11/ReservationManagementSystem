namespace ReservationManagementSystem.Application.DTOs.Account;

public sealed record RefreshToken
{
    public int Id { get; init; }
    public required string Token { get; init; }
    public DateTime Expires { get; init; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; init; }
    public string? CreatedByIp { get; init; }
    public DateTime? Revoked { get; init; }
    public string? RevokedByIp { get; init; }
    public string? ReplacedByToken { get; init; }
    public bool IsActive => Revoked == null && !IsExpired;
}
