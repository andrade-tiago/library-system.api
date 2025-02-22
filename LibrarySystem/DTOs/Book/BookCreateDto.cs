using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants.ResponseMessages;

namespace LibrarySystem.DTOs.Book;

public class BookCreateDto
{
    [Required(ErrorMessage = BookMessages.TitleRequired)]
    public string Title { get; set; }

    [Required(ErrorMessage = BookMessages.ReleaseDateRequired)]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = BookMessages.AuthorRequired)]
    [MinLength(1, ErrorMessage = BookMessages.AuthorRequired)]
    public List<int> AuthorIds { get; set; }
}
