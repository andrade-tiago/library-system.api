namespace LibrarySystem.DTOs.Request;

public class PaginationOptions
{
    public static readonly PaginationOptions Default = new();

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
