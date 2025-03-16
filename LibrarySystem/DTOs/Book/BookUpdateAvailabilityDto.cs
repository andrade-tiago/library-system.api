using LibrarySystem.Constants;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Book;

public class BookUpdateAvailabilityDto
{
    [Required(ErrorMessage = ResponseMessages.BookAvailabilityRequired)]
    public bool IsAvailable { get; set; }
}
