namespace LibraryManagementSystem.Exceptions;

public class ValidationException(string details)
    : LibraryManagementSystemException(
        StatusCodes.Status400BadRequest,
        "Request parameters are not valid",
        details
    )
{
}
