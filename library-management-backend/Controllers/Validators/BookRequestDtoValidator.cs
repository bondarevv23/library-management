using FluentValidation;
using LibraryManagementSystem.Dto.Requests;
using static LibraryManagementSystem.Constants.ValidationConstants;

namespace LibraryManagementSystem.Controllers.Validators;

public class BookRequestDtoValidator : AbstractValidator<BookRequestDto>
{
    public BookRequestDtoValidator()
    {
        RuleFor(request => request.Title)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage($"{BODY_TITLE_FILED} {NOT_NULL_CONSTRAINT}")
            .Length(BOOK_TITLE_MIN_LENGTH, BOOK_TITLE_MAX_LENGTH)
            .WithMessage(
                $"{BODY_TITLE_FILED} {string.Format(MIN_MAX_LENGTH_CONSTRAINT, BOOK_TITLE_MIN_LENGTH, BOOK_TITLE_MAX_LENGTH)}"
            );

        RuleFor(request => request.PublicationYear)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage($"{BODY_PUBLICATION_YEAR_FIELD} {NOT_NULL_CONSTRAINT}")
            .GreaterThan(0)
            .WithMessage($"{BODY_PUBLICATION_YEAR_FIELD} {POSITIVE_CONSTRAINT}");

        RuleFor(request => request.AuthorId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage($"{BODY_AUTHOR_ID_FIELD} {NOT_NULL_CONSTRAINT}")
            .GreaterThan(0)
            .WithMessage($"{BODY_AUTHOR_ID_FIELD} {POSITIVE_CONSTRAINT}");
    }
}
