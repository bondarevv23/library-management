using FluentValidation;
using LibraryManagementSystem.Exceptions;

using static LibraryManagementSystem.Constants.StringConstants;

namespace LibraryManagementSystem.Services;

public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void AssertValid<T>(T value, string instanceName)
    {
        var validator = _serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T>
            ?? throw new InternalServerErrorException($"Unknown type of validator requested: {typeof(T)}");

        var result = validator.Validate(value);
        var validationErrors = result.Errors
            .Select(error => $"'{error.PropertyName}' {error.ErrorMessage}")
            .OrderBy(errorMessage => errorMessage)
            .ToList();

        if (validationErrors.Count != 0) {
            var joinedErrors = string.Join(COMMA_DELIMITER, validationErrors);
            throw new Exceptions.ValidationException($"{instanceName} is not valid: {joinedErrors}");  
        }
    }
}
