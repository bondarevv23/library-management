using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class FindAllAuthorsRequestModelValidator : AbstractValidator<FindAllAuthorsRequestModel>
{
    public FindAllAuthorsRequestModelValidator()
    {
        RuleFor(request => request)
            .SetValidator(new IPagableValidator());
    }
}
