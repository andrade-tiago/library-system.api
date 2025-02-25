using AutoMapper;
using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Author;

namespace LibrarySystem.Services.Author;

public class AuthorService(
    IAuthorRepository authorRepository,
    IMapper mapper
) : IAuthorService
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IMapper _mapper = mapper;

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
        response.Result  = _mapper.Map<AuthorDto>(author);
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
        response.Result  = _mapper.Map<List<AuthorDto>>(authors);

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
        response.Result  = _mapper.Map<List<AuthorDto>>(authors);

        return response;
    }

    public async Task<ApiResponse<AuthorDto>> CreateAsync(AuthorCreateDto createDto)
    {
        ApiResponse<AuthorDto> response = new();

        var author = _mapper.Map<Models.Author>(createDto);
        await _authorRepository.CreateAsync(author);

        response.Message = "Author created successfully";
        response.Result  = _mapper.Map<AuthorDto>(author);
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

        _mapper.Map(updateDto, author);
        await _authorRepository.UpdateAsync(author);

        response.Message = "Author updated successfully";
        response.Result  = _mapper.Map<AuthorDto>(author);
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
