using AutoMapper;
using LibrarySystem.Constants.ResponseMessages;
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
            response.Message = AuthorMessages.NotFound;
            return response;
        }
        response.Message = AuthorMessages.Fetched;
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
            response.Message = BookMessages.NotFound;
            return response;
        }
        response.Message = AuthorMessages.FetchedMany;
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
            response.Message = AuthorMessages.EmptyPage;
            response.Result  = [];
            return response;
        }
        var authors = await _authorRepository.GetAuthorsAsync(pagination.Page, pagination.PageSize);

        response.Message = AuthorMessages.FetchedMany;
        response.Result  = _mapper.Map<List<AuthorDto>>(authors);
        return response;
    }

    public async Task<ApiResponse<AuthorDto>> CreateAsync(AuthorCreateDto createDto)
    {
        ApiResponse<AuthorDto> response = new();

        var author = _mapper.Map<Models.Author>(createDto);
        var createdAuthor = await _authorRepository.CreateAsync(author);

        if (createdAuthor is null)
        {
            response.Success = false;
            response.Message = AuthorMessages.NotCreated;
            return response;
        }
        response.Message = AuthorMessages.Created;
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
            response.Message = AuthorMessages.NotFound;
            return response;
        }
        _mapper.Map(updateDto, author);
        var updatedAuthor = await _authorRepository.UpdateAsync(author);

        if (updatedAuthor is null)
        {
            response.Success = false;
            response.Message = AuthorMessages.NotUpdated;
            return response;
        }
        response.Message = AuthorMessages.Updated;
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
