using LibrarySystem.DTOs.Request;

namespace LibrarySystem.DTOs.Book;

public class BookGetByAuthorPagedDto : PaginationRequest
{
    public bool IncludeAuthors { get; set; } = false;
}
