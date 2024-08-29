namespace ReservationManagementSystem.Application.Enums;

public enum ErrorType
{
    ValidationError = 1,
    NotFoundError,
    NoAvailableOptionsError,
    AlreadyCreatedError,
    InvalidDataPassedError,
    ExceedingNumberOfRooms,
    EmailNotSentError,
    Unauthorized,
    Forbidden,
    InvalidCredentials,
    None
}
