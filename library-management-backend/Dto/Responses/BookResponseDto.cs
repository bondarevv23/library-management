namespace LibraryManagementSystem.Dto.Responses;

public record BookResponseDto
(
    long Id,
    string Title,
    int PublicationYear,
    long AuthorId
)
{
}
