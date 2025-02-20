using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto;
using LibraryManagementSystem.Dto.Responses;

namespace LibraryManagementSystem.Services;

public interface IAuthorService
{
    Task<AuthorResponseDto> Create(CreateAuthorRequestModel request);

    Task<AuthorResponseDto> FindById(FindAuthorByIdRequestModel request);

    Task<PagedResponseDto<AuthorResponseDto>> FindAll(FindAllAuthorsRequestModel request);

    Task UpdateById(UpdateAuthorByIdRequestModel request);

    Task<AuthorResponseDto> DeleteById(DeleteAuthorByIdRequestModel request);

    Task<IList<BookResponseDto>> FindAllBooksById(FindAllBooksByAuthorIdRequestModel request);
}
