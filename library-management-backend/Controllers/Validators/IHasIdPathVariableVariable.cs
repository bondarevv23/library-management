using FluentValidation;
using LibraryManagementSystem.Controllers.Models;
using static LibraryManagementSystem.Constants.ValidationConstants;
using static LibraryManagementSystem.Utilities.ValidationUtilities;

namespace LibraryManagementSystem.Controllers.Validators;

public class IHasIdPathVariableValidator : AbstractValidator<IHasIdPathVariable>
{
    public IHasIdPathVariableValidator()
    {
        RuleFor(hasIdPathVariable => hasIdPathVariable.IdPathVariable)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage($"{BODY_FIELD} {ID_PATH_VARIABLE_FIELD}")
            .Must(BePositiveValidLong)
            .WithMessage($"{ID_PATH_VARIABLE_FIELD} {POSITIVE_VALID_LONG_CONSTRAINT}");
    }
}
