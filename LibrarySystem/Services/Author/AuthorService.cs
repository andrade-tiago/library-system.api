using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Models;
using LibrarySystem.Repositories.Author;

namespace LibrarySystem.Services.Author;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<ApiResponse<AuthorDto?>> GetByIdAsync(int id)
    {
        ApiResponse<AuthorDto?> response = new();

        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            response.Success = false;
            response.Message = "Author not found";
            return response;
        }

        response.Message = "Author successfully found";
        response.Result  = new AuthorDto
        {
            Id   = author.Id,
            Name = author.Name,
        };
        return response;
    }

    public async Task<ApiResponse<List<AuthorDto>?>> GetByBookIdAsync(int bookId)
    {
        ApiResponse<List<AuthorDto>?> response = new();

        var authors = await _authorRepository.GetByBookIdAsync(bookId);
        if (authors == null)
        {
            response.Success = false;
            response.Message = "Book not found";
            return response;
        }

        response.Message = "Authors successfully found";
        response.Result  = authors.Select(a =>
            new AuthorDto
            {
                Id   = a.Id,
                Name = a.Name,
            }
        ).ToList();

        return response;
    }

    public async Task<ApiResponse<List<AuthorDto>>> GetAuthorsAsync(PaginationRequest pagination)
    {
        ApiResponse<List<AuthorDto>> response = new();

        var totalCount = await _authorRepository.GetTotalCountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = totalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            response.Message = "No authors found for this page";
            response.Result = [];
            return response;
        }
        var authors = await _authorRepository.GetAuthorsAsync(pagination.Page, pagination.PageSize);

        response.Message = "Authors fetched successfully";
        response.Result  = authors.Select(a =>
            new AuthorDto
            {
                Id   = a.Id,
                Name = a.Name,
            }
        ).ToList();

        return response;
    }

    public async Task<ApiResponse<AuthorDto>> CreateAsync(AuthorCreateDto createDto)
    {
        ApiResponse<AuthorDto> response = new();

        var author = new Models.Author
        {
            Name = createDto.Name,
        };
        await _authorRepository.CreateAsync(author);

        response.Message = "Author created successfully";
        response.Result  = new AuthorDto
        {
            Id   = author.Id,
            Name = author.Name,
        };
        return response;
    }

    public async Task<ApiResponse<AuthorDto?>> UpdateAsync(int id, AuthorUpdateDto updateDto)
    {
        ApiResponse<AuthorDto?> response = new();

        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            response.Success = false;
            response.Message = "Author not found";
            return response;
        }

        author.Name = updateDto.Name;
        await _authorRepository.UpdateAsync(author);

        response.Message = "Author updated successfully";
        response.Result  = new AuthorDto
        {
            Id   = author.Id,
            Name = author.Name,
        };
        return response;
    }

    public async Task<bool> AuthorExistsAsync(int authorId)
    {
        return await _authorRepository.AuthorExistsAsync(authorId);
    }

    public async Task<List<Models.Author>> GetByIdsAsync(IEnumerable<int> authorIds)
    {
        return await _authorRepository.GetByIdsAsync(authorIds);
    }
}
