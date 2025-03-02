using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants;

namespace LibrarySystem.DTOs.Book;

public class BookCreateDto
{
    [Required(ErrorMessage = ResponseMessages.BookTitleRequired)]
    public string Title { get; set; }

    [Required(ErrorMessage = ResponseMessages.BookReleaseDateRequired)]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = ResponseMessages.BookAuthorsRequired)]
    [MinLength(1, ErrorMessage = ResponseMessages.BookAuthorsRequired)]
    public List<int> AuthorIds { get; set; }
}
