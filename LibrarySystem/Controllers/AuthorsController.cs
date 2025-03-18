using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Enums;
using LibrarySystem.Filters;
using LibrarySystem.Services.Author;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    private readonly IAuthorService _authorService = authorService;

    [HttpGet("{id}", Name = "GetAuthorById")]
    [ValidateIdFilter]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _authorService.GetByIdAsync(id);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/books/{bookId}/authors")]
    [ValidateIdFilter(nameof(bookId))]
    public async Task<IActionResult> GetByBookIdAsync(int bookId)
    {
        var response = await _authorService.GetByBookIdAsync(bookId);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPagedAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _authorService.GetAllPagedAsync(pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AuthorCreateDto createDto)
    {
        var response = await _authorService.CreateAsync(createDto);

        return response.Result is not null
            ? CreatedAtRoute("GetAuthorById", new { id = response.Result.Id }, response)
            : StatusCode(500, response);
    }

    [HttpPut("{id}")]
    [ValidateIdFilter]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorUpdateDto updateDto)
    {
        var response = await _authorService.UpdateAsync(id, updateDto);

        return response.Code switch
        {
            ResponseCode.AuthorUpdated
                => Ok(response),

            ResponseCode.AuthorNotFound
                => NotFound(response),

            _ => StatusCode(500, response),
        };
    }
}
