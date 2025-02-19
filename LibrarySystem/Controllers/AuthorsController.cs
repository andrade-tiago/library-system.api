using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Services.Author;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    private readonly IAuthorService _authorService = authorService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GeyByIdAsync(int id)
    {
        var response = await _authorService.GetByIdAsync(id);

        return response.Result != null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/books/{bookId}/authors")]
    public async Task<IActionResult> GetByBookIdAsync(int bookId)
    {
        var response = await _authorService.GetByBookIdAsync(bookId);

        return response.Result != null ? Ok(response) : NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAuthors([FromQuery] PaginationRequest pagination)
    {
        var response = await _authorService.GetAuthorsAsync(pagination);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AuthorCreateDto createDto)
    {
        var response = await _authorService.CreateAsync(createDto);

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AuthorUpdateDto updateDto)
    {
        var response = await _authorService.UpdateAsync(id, updateDto);

        return response.Result != null ? Ok(response) : NotFound(response);
    }
}
