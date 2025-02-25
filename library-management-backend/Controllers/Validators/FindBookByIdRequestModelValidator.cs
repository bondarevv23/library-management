using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class FindBookByIdRequestModelValidator : AbstractValidator<FindBookByIdRequestModel>
{
    public FindBookByIdRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IHasIdPathVariableValidator());
    }
}
