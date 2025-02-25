namespace LibraryManagementSystem.Exceptions;

public class LibraryManagementSystemException(int code, string message, string details)
    : Exception(message)
{
    public int Code { get; init; } = code;
    public string LogDescription { get; set; } = details;
}
