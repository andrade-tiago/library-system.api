namespace LibrarySystem.DTOs.Book;

public class BookGetAllPagedOptions
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool IncludeAuthors { get; set; }
}
