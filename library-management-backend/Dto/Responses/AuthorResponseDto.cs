namespace LibraryManagementSystem.Dto.Responses;

public record AuthorResponseDto
(
    long Id,
    string Name,
    DateOnly DateOfBirth
)
{
}
