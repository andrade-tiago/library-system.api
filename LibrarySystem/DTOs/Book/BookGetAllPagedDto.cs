using LibrarySystem.DTOs.Request;

namespace LibrarySystem.DTOs.Book;

public class BookGetAllPagedDto : PaginationOptions
{
    public bool IncludeAuthors { get; set; } = false;
}
