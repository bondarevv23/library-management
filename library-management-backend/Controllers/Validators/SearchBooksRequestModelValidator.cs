using FluentValidation;
using LibraryManagementSystem.Controllers.Models;
using static LibraryManagementSystem.Constants.ValidationConstants;

namespace LibraryManagementSystem.Controllers.Validators;

public class SearchBooksRequestModelValidator : AbstractValidator<SearchBooksRequestModel>
{
    public SearchBooksRequestModelValidator()
    {
        RuleFor(request => request.UrlEncodedSearchQuery)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage($"{URI_VARIABLE_QUERY_FIELD} {NOT_NULL_CONSTRAINT}")
            .Length(QUERY_MIN_LENGTH, QUERY_MAX_LENGTH)
            .WithMessage(
                $"{URI_VARIABLE_QUERY_FIELD} {string.Format(MIN_MAX_LENGTH_CONSTRAINT, QUERY_MIN_LENGTH, QUERY_MAX_LENGTH)}"
            );

        RuleFor(request => request).Cascade(CascadeMode.Stop).SetValidator(new IPagableValidator());
    }
}
