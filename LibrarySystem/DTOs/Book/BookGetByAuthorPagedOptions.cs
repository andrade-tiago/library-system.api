namespace LibrarySystem.DTOs.Book;

public class BookGetByAuthorPagedOptions
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool IncludeAuthors { get; set; }
}
