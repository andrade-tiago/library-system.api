using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Author;
using LibrarySystem.Repositories.Book;

namespace LibrarySystem.Services.Author;

public class AuthorService(
    IAuthorRepository authorRepository,
    IBookRepository bookRepository,
    IMapper mapper
) : IAuthorService
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<AuthorDto?>> GetByIdAsync(int id)
    {
        ApiResponse<AuthorDto?> response = new();

        var author = await _authorRepository.GetByIdAsync(id);
        if (author is null)
        {
            _mapper.Map(ResponseStatus.AuthorNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.AuthorFetched, response);
        response.Result = _mapper.Map<AuthorDto>(author);
        return response;
    }

    public async Task<ApiResponse<List<AuthorDto>?>> GetByBookIdAsync(int bookId)
    {
        ApiResponse<List<AuthorDto>?> response = new();

        var bookExists = await _bookRepository.BookExistsAsync(bookId);

        if (!bookExists)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        var authors = await _authorRepository.GetByBookIdAsync(bookId);

        _mapper.Map(ResponseStatus.AuthorFetchedMany, response);
        response.Result = _mapper.Map<List<AuthorDto>>(authors);
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
            _mapper.Map(ResponseStatus.AuthorEmptyPage, response);
            response.Result = [];
            return response;
        }
        var authors = await _authorRepository.GetAuthorsAsync(pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.AuthorFetchedMany, response);
        response.Result = _mapper.Map<List<AuthorDto>>(authors);
        return response;
    }

    public async Task<ApiResponse<AuthorDto>> CreateAsync(AuthorCreateDto createDto)
    {
        ApiResponse<AuthorDto> response = new();

        var author = _mapper.Map<Models.Author>(createDto);
        var createdAuthor = await _authorRepository.CreateAsync(author);

        if (createdAuthor is null)
        {
            _mapper.Map(ResponseStatus.AuthorNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.AuthorCreated, response);
        response.Result = _mapper.Map<AuthorDto>(author);
        return response;
    }

    public async Task<ApiResponse<AuthorDto?>> UpdateAsync(int id, AuthorUpdateDto updateDto)
    {
        ApiResponse<AuthorDto?> response = new();

        var author = await _authorRepository.GetByIdAsync(id);
        if (author is null)
        {
            _mapper.Map(ResponseStatus.AuthorNotFound, response);
            return response;
        }
        _mapper.Map(updateDto, author);
        var updatedAuthor = await _authorRepository.UpdateAsync(author);

        if (updatedAuthor is null)
        {
            _mapper.Map(ResponseStatus.AuthorNotUpdated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.AuthorUpdated, response);
        response.Result = _mapper.Map<AuthorDto>(author);
        return response;
    }
}
