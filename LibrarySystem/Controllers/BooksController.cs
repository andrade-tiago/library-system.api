using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Services.Book;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

    [HttpGet("{id}", Name = nameof(GetByIdAsync))]
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

    [HttpGet("/authors/{authorId}/books")]
    public async Task<IActionResult> GetByAuthorIdAsync(int authorId, [FromQuery] PaginationRequest pagination)
    {
        var response = await _bookService.GetByAuthorIdAsync(authorId, pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookAsync(BookCreateDto dto)
    {
        var response = await _bookService.CreateBookAsync(dto);

        return response.Result is not null
            ? CreatedAtRoute(nameof(GetByIdAsync), new { id = response.Result.Id }, response)
            : BadRequest(response);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBookAsync(int bookId, BookUpdateDto dto)
    {
        var response = await _bookService.UpdateBookAsync(bookId, dto);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }
}
