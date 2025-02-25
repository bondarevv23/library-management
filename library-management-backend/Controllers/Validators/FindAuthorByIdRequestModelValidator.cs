using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class FindAuthorByIdRequestModelValidator : AbstractValidator<FindAuthorByIdRequestModel>
{
    public FindAuthorByIdRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IHasIdPathVariableValidator());
    }
}
