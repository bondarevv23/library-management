namespace LibraryManagementSystem.Exceptions;

public class NotFoundException(string details)
    : LibraryManagementSystemException(
        StatusCodes.Status404NotFound,
        "No resource found by provided parameters",
        details
    )
{
    public NotFoundException(string tableName, long id)
        : this($"No '{tableName}' found by id = '{id}'") { }
}
