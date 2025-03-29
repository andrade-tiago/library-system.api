using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Book;

public interface IBookService 
{
    Task<ApiResponse<BookDto?>> GetByIdAsync(int id, BookGetByIdDto options);
    Task<ApiResponse<List<BookDto>>> GetAllPagedAsync(BookGetAllPagedDto options);
    Task<ApiResponse<List<BookDto>?>> GetByAuthorPagedAsync(int authorId, BookGetByAuthorPagedDto options);
    Task<ApiResponse<BookDto?>> CreateAsync(BookCreateDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookBasicAsync(int bookId, BookUpdateBasicDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAuthorsAsync(int bookId, BookUpdateAuthorsDto dto);
    Task<ApiResponse<BookDto?>> UpdateBookAvailabilityAsync(int bookId, BookUpdateAvailabilityDto dto);
}
