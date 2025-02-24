using LibrarySystem.Constants.ResponseMessages;
using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Mappers;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Services.Author;

namespace LibrarySystem.Services.Book;

public class BookService(
    IBookRepository bookRepository,
    IAuthorService authorService
) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IAuthorService _authorService = authorService;

    public async Task<ApiResponse<BookDto?>> GetByIdAsync(int id)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            response.Success = false;
            response.Message = BookMessages.NotFound;
            return response;
        }
        response.Message = BookMessages.BookFoundSuccessfully;
        response.Result = BookMapper.ToDto(book);
        return response;
    }

    public async Task<ApiResponse<List<BookDto>>> GetBooksAsync(PaginationRequest pagination)
    {
        ApiResponse<List<BookDto>> response = new();

        var totalCount = await _bookRepository.GetTotalCountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems = totalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            response.Message = BookMessages.PageEmpty;
            response.Result = [];
            return response;
        }
        var books = await _bookRepository.GetBooksAsync(pagination.Page, pagination.PageSize);

        response.Message = BookMessages.BooksFoundSuccessfully;
        response.Result = books.Select(b => BookMapper.ToDto(b)).ToList();
        return response;
    }

    public async Task<ApiResponse<List<BookDto>?>> GetByAuthorIdAsync(int authorId, PaginationRequest pagination)
    {
        ApiResponse<List<BookDto>?> response = new();

        var authorExists = await _authorService.AuthorExistsAsync(authorId);

        if (!authorExists)
        {
            response.Success = false;
            response.Message = AuthorMessages.NotFound;
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
            response.Result  = [];
            response.Message = totalCount > 0
                ? BookMessages.PageEmpty
                : BookMessages.NoAuthorBooks;
            return response;
        }
        var books = await _bookRepository.GetByAuthorIdAsync(authorId, pagination.Page, pagination.PageSize);

        response.Message = BookMessages.BooksFoundSuccessfully;
        response.Result  = books.Select(b => BookMapper.ToDto(b)).ToList();
        return response;
    }

    public async Task<ApiResponse<BookDto?>> CreateBookAsync(BookCreateDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var authors = await _authorService.GetByIdsAsync(dto.AuthorIds);

        if (authors.Count != dto.AuthorIds.Count)
        {
            response.Success = false;
            response.Message = AuthorMessages.AuthorsNotFound;
            return response;
        }
        var book = new Models.Book
        {
            Title       = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            Authors     = authors,
        };
        await _bookRepository.CreateBookAsync(book);

        response.Message = BookMessages.Created;
        response.Result  = BookMapper.ToDto(book);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> UpdateBookAsync(int bookId, BookUpdateDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book is null)
        {
            response.Success = false;
            response.Message = BookMessages.NotFound;
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
            var authorsToAdd = await _authorService.GetByIdsAsync(authorIdsToAdd);

            if (authorsToAdd.Count != authorIdsToAdd.Count)
            {
                response.Success = false;
                response.Message = AuthorMessages.AuthorsNotFound;
                return response;
            }
            book.Authors.AddRange(authorsToAdd);
        }

        book.Title       = dto.Title;
        book.ReleaseDate = dto.ReleaseDate;
        await _bookRepository.UpdateBookAsync(book);

        response.Message = BookMessages.Updated;
        response.Result  = BookMapper.ToDto(book);
        return response;
    }
}
