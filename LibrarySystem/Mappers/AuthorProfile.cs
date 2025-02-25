using AutoMapper;
using LibrarySystem.DTOs.Author;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<AuthorCreateDto, Author>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AuthorUpdateDto, Author>()
            .ForMember(dest => dest.Id,    opt => opt.Ignore())
            .ForMember(dest => dest.Books, opt => opt.Ignore());

        CreateMap<Author, AuthorDto>();
    }
}
