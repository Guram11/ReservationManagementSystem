using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class AccountServiceErrors
{
    public static Error UserNotFound(string email) => new Error(
        "Account.UserNotFound", $"No accounts registered with the email '{email}'.");

    public static Error InvalidCredentials(string email) => new Error(
        "Account.InvalidCredentials", $"Invalid credentials for '{email}'.");

    public static Error EmailNotConfirmed(string email) => new Error(
        "Account.EmailNotConfirmed", $"Account not confirmed for '{email}'.");

    public static Error TokenGenerationError(string message) => new Error(
        "Account.TokenGenerationError", $"Error while generating JWT token: {message}.");

    public static Error UsernameTaken(string username) => new Error(
        "Account.UsernameTaken", $"Username '{username}' is already taken.");

    public static Error PasswordsDoNotMatch() => new Error(
    "Account.PasswordsDoNotMatch", $"Passwords do not match!");

    public static Error EmailRegistered(string email) => new Error(
        "Account.EmailRegistered", $"Email '{email}' is already registered.");

    public static Error EmailConfirmationFailed(string email) => new Error(
        "Account.EmailConfirmationFailed", $"An error occurred while confirming the email '{email}'.");

    public static Error PasswordResetFailed(string email) => new Error(
        "Account.PasswordResetFailed", $"Error occurred while resetting the password for '{email}'.");
}
