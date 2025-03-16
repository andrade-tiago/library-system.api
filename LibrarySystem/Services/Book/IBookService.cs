using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Book;

public interface IBookService 
{
    Task<ApiResponse<BookDto?>> GetByIdAsync(int id);
    Task<ApiResponse<List<BookDto>>> GetBooksAsync(PaginationRequest pagination);
    Task<ApiResponse<List<BookDto>?>> GetByAuthorIdAsync(int authorId, PaginationRequest pagination);
    Task<ApiResponse<BookDto?>> CreateBookAsync(BookCreateDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookBasicAsync(int bookId, BookUpdateBasicDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAuthorsAsync(int bookId, BookUpdateAuthorsDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAvailabilityAsync(int bookId, BookUpdateAvailabilityDto dto);
}
