namespace LibrarySystem.DTOs.Response;

public class Pagination
{
    public int CurrentPage { get; init; }
    public int TotalItems { get; init; }
    public int ItemsPerPage { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
}
