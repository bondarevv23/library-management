using FluentValidation;
using LibraryManagementSystem.Controllers.Models;
using static LibraryManagementSystem.Constants.ValidationConstants;

namespace LibraryManagementSystem.Controllers.Validators;

public class CreateAuthorRequestModelValidator : AbstractValidator<CreateAuthorRequestModel>
{
    public CreateAuthorRequestModelValidator()
    {
        RuleFor(request => request.Body)
            .NotNull()
            .WithMessage($"{BODY_FIELD} {NOT_NULL_CONSTRAINT}")
            .DependentRules(() =>
            {
                RuleFor(request => request.Body!).SetValidator(new AuthorRequestDtoValidator());
            });
    }
}
