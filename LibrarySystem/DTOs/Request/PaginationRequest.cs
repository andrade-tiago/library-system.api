using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants.ResponseMessages;

namespace LibrarySystem.DTOs.Request;

public class PaginationRequest
{
    [Range(
        minimum: 1,
        maximum: int.MaxValue,
        ErrorMessage = PaginationMessages.PageOutOfRange
    )]
    public int Page { get; set; } = 1;

    [Range(
        minimum: 1,
        maximum: 100,
        ErrorMessage = PaginationMessages.PageSizeOutOfRange
    )]
    public int PageSize { get; set; } = 10;
}
