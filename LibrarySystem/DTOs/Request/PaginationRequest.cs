using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants;

namespace LibrarySystem.DTOs.Request;

public class PaginationRequest
{
    [Range(
        minimum: 1,
        maximum: int.MaxValue,
        ErrorMessage = ResponseMessages.PageIndexOutOfRange
    )]
    public int Page { get; set; } = 1;

    [Range(
        minimum: 1,
        maximum: 100,
        ErrorMessage = ResponseMessages.PageSizeOutOfRange
    )]
    public int PageSize { get; set; } = 10;
}
