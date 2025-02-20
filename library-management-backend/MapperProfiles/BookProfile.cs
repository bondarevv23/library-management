using AutoMapper;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.MapperProfiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookRequestDto, Book>();
        CreateMap<Book, BookResponseDto>();
    }
}
