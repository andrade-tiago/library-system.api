using LibrarySystem.DTOs.Request;

namespace LibrarySystem.DTOs.Book;

public class BookGetAllPagedDto : PaginationRequest
{
    public bool IncludeAuthors { get; set; } = false;
}
