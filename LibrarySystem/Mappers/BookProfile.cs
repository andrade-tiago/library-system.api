using AutoMapper;
using LibrarySystem.DTOs.Book;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.Id,      opt => opt.Ignore())
            .ForMember(dest => dest.Authors, opt => opt.Ignore());

        CreateMap<BookUpdateBasicDto, Book>()
            .ForMember(dest => dest.Id,          opt => opt.Ignore())
            .ForMember(dest => dest.Authors,     opt => opt.Ignore())
            .ForMember(dest => dest.IsAvailable, opt => opt.Ignore());

        CreateMap<Book, BookDto>();

        CreateMap<BookGetByIdDto, BookGetByIdOptions>();
        CreateMap<BookGetAllPagedDto, BookGetAllPagedOptions>();
        CreateMap<BookGetByAuthorPagedDto, BookGetByAuthorPagedOptions>();
    }
}
