using AutoMapper;

namespace LibraryManagementSystem.MapperProfiles;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
    }
}
