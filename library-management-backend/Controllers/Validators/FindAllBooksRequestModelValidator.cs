using FluentValidation;
using LibraryManagementSystem.Controllers.Models;

namespace LibraryManagementSystem.Controllers.Validators;

public class FindAllBooksRequestModelValidator : AbstractValidator<FindAllBooksRequestModel>
{
    public FindAllBooksRequestModelValidator()
    {
        RuleFor(request => request).SetValidator(new IPagableValidator());
    }
}
