using LibrarySystem.DTOs.Author;

namespace LibrarySystem.DTOs.Book;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<AuthorDto> Authors { get; set; }
}
