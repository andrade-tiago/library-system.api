using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Request;

public class PaginationRequest
{
    [Range(
        minimum: 1,
        maximum: int.MaxValue,
        ErrorMessage = "Page must be at least 1."
    )]
    public int Page { get; set; } = 1;

    [Range(
        minimum: 1,
        maximum: 100,
        ErrorMessage = "PageSize must be between 1 and 100."
    )]
    public int PageSize { get; set; } = 10;
}
