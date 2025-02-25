using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class DeleteBookByIdRequestModelValidator : AbstractValidator<DeleteBookByIdRequestModel>
{
    public DeleteBookByIdRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IHasIdPathVariableValidator());
    }
}
