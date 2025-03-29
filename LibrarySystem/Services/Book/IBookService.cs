using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Book;

public interface IBookService 
{
    Task<ApiResponse<BookDto?>> GetByIdAsync(int id, BookQueryOptions queryOptions);
    Task<ApiResponse<List<BookDto>>> GetAllPagedAsync(BookQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<ApiResponse<List<BookDto>?>> GetByAuthorPagedAsync(int authorId, BookQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<ApiResponse<BookDto?>> CreateAsync(BookCreateDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookBasicAsync(int bookId, BookUpdateBasicDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAuthorsAsync(int bookId, BookUpdateAuthorsDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAvailabilityAsync(int bookId, BookUpdateAvailabilityDto dto);
}
