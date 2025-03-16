namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public List<Author> Authors { get; set; }
    public bool IsAvailable { get; set; } = true;
}
