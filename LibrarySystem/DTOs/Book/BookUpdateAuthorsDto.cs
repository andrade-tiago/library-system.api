using LibrarySystem.Constants;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Book;

public class BookUpdateAuthorsDto
{
    [Required(ErrorMessage = ResponseMessages.BookAuthorsRequired)]
    [MinLength(1, ErrorMessage = ResponseMessages.BookAuthorsRequired)]
    public List<int> AuthorIds { get; set; }
}
