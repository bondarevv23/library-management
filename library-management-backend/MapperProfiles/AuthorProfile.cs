using AutoMapper;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.MapperProfiles;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<AuthorRequestDto, Author>();
        CreateMap<Author, AuthorResponseDto>();
    }
}
