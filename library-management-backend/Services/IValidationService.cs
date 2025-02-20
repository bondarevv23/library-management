namespace LibraryManagementSystem.Services;

public interface IValidationService
{
    void AssertValid<T> (T value, string instanceName);
}
