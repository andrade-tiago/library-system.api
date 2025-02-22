using LibrarySystem.DTOs.Author;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class AuthorMapper
{
    public static AuthorDto ToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
        };
    }
}
