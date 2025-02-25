using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class FindAllBooksByAuthorIdRequestModelValidator
    : AbstractValidator<FindAllBooksByAuthorIdRequestModel>
{
    public FindAllBooksByAuthorIdRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IHasIdPathVariableValidator());
    }
}
