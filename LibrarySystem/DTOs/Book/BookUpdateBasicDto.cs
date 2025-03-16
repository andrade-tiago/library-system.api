using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants;

namespace LibrarySystem.DTOs.Book;

public class BookUpdateBasicDto
{
    [Required(ErrorMessage = ResponseMessages.BookTitleRequired)]
    public string Title { get; set; }

    [Required(ErrorMessage = ResponseMessages.BookReleaseDateRequired)]
    public DateOnly ReleaseDate { get; set; }
}
