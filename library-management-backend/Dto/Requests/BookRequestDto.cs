namespace LibraryManagementSystem.Dto.Requests;

public record BookRequestDto(string? Title, int? PublicationYear, long? AuthorId) { }
