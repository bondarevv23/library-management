namespace LibraryManagementSystem.Exceptions;

public class InternalServerErrorException(string details)
    : LibraryManagementSystemException(
        StatusCodes.Status500InternalServerError,
        "Internal server error",
        details
    )
{
}