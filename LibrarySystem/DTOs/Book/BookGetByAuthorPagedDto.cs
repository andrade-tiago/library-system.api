using LibrarySystem.DTOs.Request;

namespace LibrarySystem.DTOs.Book;

public class BookGetByAuthorPagedDto : PaginationOptions
{
    public bool IncludeAuthors { get; set; } = false;
}
