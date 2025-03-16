using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Enums;
using LibrarySystem.Filters;
using LibrarySystem.Services.Book;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

    [HttpGet("{id}", Name = "GetBookById")]
    [ValidateIdFilter]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _bookService.GetByIdAsync(id);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetBooksAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _bookService.GetBooksAsync(pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/authors/{authorId}/books")]
    [ValidateIdFilter(nameof(authorId))]
    public async Task<IActionResult> GetByAuthorIdAsync(int authorId, [FromQuery] PaginationRequest pagination)
    { 
        var response = await _bookService.GetByAuthorIdAsync(authorId, pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookAsync(BookCreateDto dto)
    {
        var response = await _bookService.CreateBookAsync(dto);

        return response.Code switch
        {
            ResponseCode.BookCreated when response.Result is not null
                => CreatedAtRoute("GetBookById", new { id = response.Result.Id }, response),

            ResponseCode.AuthorNotFoundSome
                => NotFound(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpPut("{id}")]
    [ValidateIdFilter]
    public async Task<IActionResult> UpdateBookBasicAsync(int id, BookUpdateBasicDto dto)
    {
        var response = await _bookService.UpdateBookBasicAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.BookUpdated when response.Result is not null
                => Ok(response),

            ResponseCode.BookNotFound
                => NotFound(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpPatch("{id}/authors")]
    [ValidateIdFilter]
    public async Task<IActionResult> UpdateBookAuthorsAsync(int id, BookUpdateAuthorsDto dto)
    {
        var response = await _bookService.UpdateBookAuthorsAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.BookUpdated when response.Result is not null
                => Ok(response),

            ResponseCode.BookNotFound or
            ResponseCode.AuthorNotFoundSome
                => NotFound(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpPatch("{id}/availability")]
    [ValidateIdFilter]
    public async Task<IActionResult> UpdateBookAvailabilityAsync(int id, BookUpdateAvailabilityDto dto)
    {
        var response = await _bookService.UpdateBookAvailabilityAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.BookUpdated when response.Result is not null
                => Ok(response),

            ResponseCode.BookNotFound
                => NotFound(response),

            ResponseCode.OpenBookReservation
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }
}
