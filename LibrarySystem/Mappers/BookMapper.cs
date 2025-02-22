using LibrarySystem.DTOs.Book;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class BookMapper
{
    public static BookDto ToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ReleaseDate = book.ReleaseDate,
            Authors = book.Authors.Select(a => AuthorMapper.ToDto(a)).ToList(),
        };
    }
}
