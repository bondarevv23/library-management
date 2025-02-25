using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto.Responses;

namespace LibraryManagementSystem.Services;

public interface IBookService
{
    Task<BookResponseDto> Create(CreateBookRequestModel request);

    Task<PagedResponseDto<BookResponseDto>> FindAll(FindAllBooksRequestModel request);

    Task<BookResponseDto> FindById(FindBookByIdRequestModel request);

    Task UpdateById(UpdateBookByIdRequestModel request);

    Task<BookResponseDto> DeleteById(DeleteBookByIdRequestModel request);

    Task<PagedResponseDto<BookResponseDto>> Search(SearchBooksRequestModel request);
}
