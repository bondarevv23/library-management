using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

using static LibraryManagementSystem.Utilities.ValidationUtilities;
using static LibraryManagementSystem.Constants.ValidationConstants;


namespace LibraryManagementSystem.Controllers.Validators;

public class IPagableValidator : AbstractValidator<IPagable>
{
    public IPagableValidator()
    {
        RuleFor(request => request.PageNumberPathVariable)
            .Must(BePositiveValidInt).WithMessage($"{PAGE_NUMBER_PATH_VARIABLE_FIELD} {POSITIVE_VALID_INT_CONSTRAINT}")
            .When(request => request.PageNumberPathVariable != null);

        RuleFor(request => request.PageSizePathVariable)
            .Must(BePositiveMultipleOf10Int).WithMessage($"{PAGE_SIZE_PATH_VARIABLE_FIELD} {POSITIVE_VALID_INT_MULTIPLE_10}")
            .When(request => request.PageSizePathVariable != null)
            .Must(value => int.TryParse(value, out var result) && result <= PAGE_SIZE_MAX_VALUE)
            .WithMessage($"{PAGE_SIZE_PATH_VARIABLE_FIELD} must not exceed {PAGE_SIZE_MAX_VALUE}")
            .When(request => request.PageSizePathVariable != null);
    }
}