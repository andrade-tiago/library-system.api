using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Author;
using LibrarySystem.Repositories.Book;

namespace LibrarySystem.Services.Book;

public class BookService(
    IBookRepository bookRepository,
    IAuthorRepository authorRepository,
    IMapper mapper
) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<BookDto?>> GetByIdAsync(int id)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookFetched, response);
        response.Result = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<List<BookDto>>> GetBooksAsync(PaginationRequest pagination)
    {
        ApiResponse<List<BookDto>> response = new();

        var totalCount = await _bookRepository.GetTotalCountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = totalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.BookPageEmpty, response);
            response.Result = [];
            return response;
        }
        var books = await _bookRepository.GetBooksAsync(pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.BookFetchedMany, response);
        response.Result = _mapper.Map<List<BookDto>>(books);
        return response;
    }

    public async Task<ApiResponse<List<BookDto>?>> GetByAuthorIdAsync(int authorId, PaginationRequest pagination)
    {
        ApiResponse<List<BookDto>?> response = new();

        var authorExists = await _authorRepository.AuthorExistsAsync(authorId);

        if (!authorExists)
        {
            _mapper.Map(ResponseStatus.AuthorNotFound, response);
            return response;
        }

        var totalCount = await _bookRepository.GetAuthorBooksTotalCountAsync(authorId);

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = totalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            var responseStatus = totalCount > 0
                ? ResponseStatus.BookPageEmpty
                : ResponseStatus.NoAuthorBooks;

            _mapper.Map(responseStatus, response);
            response.Result = [];
            return response;
        }
        var books = await _bookRepository.GetByAuthorIdAsync(authorId, pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.BookFetchedMany, response);
        response.Result = _mapper.Map<List<BookDto>>(books);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> CreateBookAsync(BookCreateDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var authors = await _authorRepository.GetByIdsAsync(dto.AuthorIds);

        if (authors.Count != dto.AuthorIds.Count)
        {
            _mapper.Map(ResponseStatus.AuthorNotFoundSome, response);
            return response;
        }
        var book = _mapper.Map<Models.Book>(dto);
        book.Authors = authors;

        var createdBook = await _bookRepository.CreateBookAsync(book);

        if (createdBook is null)
        {
            _mapper.Map(ResponseStatus.BookNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookCreated, response);
        response.Result  = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> UpdateBookAsync(int bookId, BookUpdateDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        var currentAuthorIds = book.Authors.Select(a => a.Id).ToHashSet();

        var authorIdsToRemove = currentAuthorIds.Except(dto.AuthorIds).ToHashSet();
        if (authorIdsToRemove.Count > 0)
        {
            book.Authors.RemoveAll(a => authorIdsToRemove.Contains(a.Id));
        }

        var authorIdsToAdd = dto.AuthorIds.Except(currentAuthorIds).ToHashSet();
        if (authorIdsToAdd.Count > 0)
        {
            var authorsToAdd = await _authorRepository.GetByIdsAsync(authorIdsToAdd);

            if (authorsToAdd.Count != authorIdsToAdd.Count)
            {
                _mapper.Map(ResponseStatus.AuthorNotFoundSome, response);
                return response;
            }
            book.Authors.AddRange(authorsToAdd);
        }
        _mapper.Map(dto, book);
        var updatedBook = await _bookRepository.UpdateBookAsync(book);

        if (updatedBook is null)
        {
            _mapper.Map(ResponseStatus.BookNotUpdated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookUpdated, response);
        response.Result = _mapper.Map<BookDto>(book);
        return response;
    }
}
