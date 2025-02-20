using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

using static LibraryManagementSystem.Constants.ValidationConstants;

namespace LibraryManagementSystem.Controllers.Validators;

public class UpdateBookByIdRequestModelValidator : AbstractValidator<UpdateBookByIdRequestModel>
{
    public UpdateBookByIdRequestModelValidator()
    {
        RuleFor(request => request)
            .Cascade(CascadeMode.Stop)
            .SetValidator(new IHasIdPathVariableValidator());

        RuleFor(request => request.Body)
            .NotNull().WithMessage($"{BODY_FIELD} {NOT_NULL_CONSTRAINT}")
            .DependentRules(() => {
                RuleFor(request => request.Body!)
                    .SetValidator(new UpdateBookRequestDtoValidator());
            });
    }
}
