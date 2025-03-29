using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Author;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Repositories.Reservation;

namespace LibrarySystem.Services.Book;

public class BookService(
    IBookRepository bookRepository,
    IAuthorRepository authorRepository,
    IReservationRepository reservationRepository,
    IMapper mapper
) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<BookDto?>> GetByIdAsync(int id, BookGetByIdDto options)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(id, _mapper.Map<BookGetByIdOptions>(options));

        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookFetched, response);
        response.Result = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<List<BookDto>>> GetAllPagedAsync(BookGetAllPagedDto options)
    {
        ApiResponse<List<BookDto>> response = new();

        var totalCount = await _bookRepository.CountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = options.Page,
            ItemsPerPage = options.PageSize,
            TotalItems   = totalCount,
        };

        if (options.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.BookPageEmpty, response);
            response.Result = [];
            return response;
        }
        var books = await _bookRepository.GetAllPagedAsync(_mapper.Map<BookGetAllPagedOptions>(options));

        _mapper.Map(ResponseStatus.BookFetchedMany, response);
        response.Result = _mapper.Map<List<BookDto>>(books);
        return response;
    }

    public async Task<ApiResponse<List<BookDto>?>> GetByAuthorPagedAsync(int authorId, BookGetByAuthorPagedDto options)
    {
        ApiResponse<List<BookDto>?> response = new();

        var authorExists = await _authorRepository.AuthorExistsAsync(authorId);

        if (!authorExists)
        {
            _mapper.Map(ResponseStatus.AuthorNotFound, response);
            return response;
        }

        var totalCount = await _bookRepository.CountByAuthorIdAsync(authorId);

        response.Pagination = new Pagination
        {
            CurrentPage  = options.Page,
            ItemsPerPage = options.PageSize,
            TotalItems   = totalCount,
        };

        if (options.Page > response.Pagination.TotalPages)
        {
            var responseStatus = totalCount > 0
                ? ResponseStatus.BookPageEmpty
                : ResponseStatus.NoAuthorBooks;

            _mapper.Map(responseStatus, response);
            response.Result = [];
            return response;
        }
        var books = await _bookRepository.GetByAuthorPagedAsync(authorId, _mapper.Map<BookGetByAuthorPagedOptions>(options));

        _mapper.Map(ResponseStatus.BookFetchedMany, response);
        response.Result = _mapper.Map<List<BookDto>>(books);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> CreateAsync(BookCreateDto dto)
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

        var createdBook = await _bookRepository.CreateAsync(book);

        if (createdBook is null)
        {
            _mapper.Map(ResponseStatus.BookNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookCreated, response);
        response.Result  = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> UpdateBookBasicAsync(int bookId, BookUpdateBasicDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(bookId, new BookGetByIdOptions { IncludeAuthors = false });
        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        _mapper.Map(dto, book);

        var updatedBook = await _bookRepository.UpdateAsync(book);
        if (updatedBook is null)
        {
            _mapper.Map(ResponseStatus.BookNotUpdated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookUpdated, response);
        response.Result = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> UpdateBookAuthorsAsync(int bookId, BookUpdateAuthorsDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(bookId, new BookGetByIdOptions { IncludeAuthors = true });
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

        var updatedBook = await _bookRepository.UpdateAsync(book);
        if (updatedBook is null)
        {
            _mapper.Map(ResponseStatus.BookNotUpdated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.BookUpdated, response);
        response.Result = _mapper.Map<BookDto>(book);
        return response;
    }

    public async Task<ApiResponse<BookDto?>> UpdateBookAvailabilityAsync(int bookId, BookUpdateAvailabilityDto dto)
    {
        ApiResponse<BookDto?> response = new();

        var book = await _bookRepository.GetByIdAsync(bookId, new BookGetByIdOptions { IncludeAuthors = false });
        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        // Cannot mark a book as unavailable if it is reserved
        if (dto.IsAvailable == false && book.IsAvailable)
        {
            var lastBookReservation = await _reservationRepository.GetLastByBookAsync(bookId);
            if (lastBookReservation is not null && lastBookReservation.ReturnedDate is null)
            {
                _mapper.Map(ResponseStatus.OpenBookReservation, response);
                return response;
            }
        }

        book.IsAvailable = dto.IsAvailable;
        var updatedBook = await _bookRepository.UpdateAsync(book);

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
