using FluentValidation;
using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Utilities.ValidationUtilities;
using static LibraryManagementSystem.Constants.ValidationConstants;

namespace LibraryManagementSystem.Controllers.Validators;

public class AuthorRequestDtoValidator : AbstractValidator<AuthorRequestDto>
{
    public AuthorRequestDtoValidator()
    {
        RuleFor(request => request.Name).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{BODY_NAME_FILED} {NOT_NULL_CONSTRAINT}")
            .Length(AUTHOR_NAME_MIN_LENGTH, AUTHOR_NAME_MAX_LENGTH).WithMessage(
                $"{BODY_NAME_FILED} {string.Format(MIN_MAX_LENGTH_CONSTRAINT, AUTHOR_NAME_MIN_LENGTH, AUTHOR_NAME_MAX_LENGTH)}"
            );

        RuleFor(request => request.DateOfBirth).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage($"{BODY_DATE_OF_BIRTH_FIELD} {NOT_NULL_CONSTRAINT}")
            .Must(BeValidDateOnly).WithMessage($"{BODY_DATE_OF_BIRTH_FIELD} must be valid DateOnly");
    }
}