namespace LibrarySystem.DTOs.Book;

public class BookQueryOptions
{
    public static readonly BookQueryOptions Default = new();

    public bool IncludeAuthors { get; set; } = false;
}
