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

        CreateMap<BookUpdateDto, Book>()
            .ForMember(dest => dest.Id,      opt => opt.Ignore())
            .ForMember(dest => dest.Authors, opt => opt.Ignore());

        CreateMap<Book, BookDto>();
    }
}
