using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class DeleteAuthorByIdRequestModelValidator : AbstractValidator<DeleteAuthorByIdRequestModel>
{
    public DeleteAuthorByIdRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IHasIdPathVariableValidator());
    }
}
