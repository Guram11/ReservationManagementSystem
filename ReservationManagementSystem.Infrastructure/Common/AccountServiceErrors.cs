using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class AccountServiceErrors
{
    public static Error UserNotFound(string email) => new Error(
        ErrorType.NotFoundError, $"No accounts registered with the email '{email}'.");

    public static Error InvalidCredentials(string email) => new Error(
        ErrorType.InvalidCredentials, $"Invalid credentials for '{email}'.");

    public static Error EmailNotConfirmed(string email) => new Error(
        ErrorType.NotFoundError, $"Account not confirmed for '{email}'.");

    public static Error TokenGenerationError(string message) => new Error(
        ErrorType.InvalidCredentials, $"Error while generating JWT token: {message}.");

    public static Error UsernameTaken(string username) => new Error(
        ErrorType.NotFoundError, $"Username '{username}' is already taken.");

    public static Error UserAlreadyInRole(string username) => new Error(
        ErrorType.AlreadyCreatedError, $"User: '{username}' is already in role.");

    public static Error PasswordsDoNotMatch() => new Error(
       ErrorType.InvalidDataPassedError, $"Passwords do not match!");

    public static Error EmailRegistered(string email) => new Error(
        ErrorType.AlreadyCreatedError, $"Email '{email}' is already registered.");

    public static Error EmailConfirmationFailed(string email) => new Error(
        ErrorType.EmailNotSentError, $"An error occurred while confirming the email '{email}'.");

    public static Error PasswordResetFailed(string email) => new Error(
        ErrorType.NotFoundError, $"Error occurred while resetting the password for '{email}'.");

    public static Error UserCreationFailed(string error) => new Error(
        ErrorType.NotFoundError, $"UserCreationFailed: '{error}'.");
}
