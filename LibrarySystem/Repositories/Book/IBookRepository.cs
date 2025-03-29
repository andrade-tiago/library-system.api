using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;

namespace LibrarySystem.Repositories.Book;

public interface IBookRepository
{
    Task<Models.Book?> GetByIdAsync(int id, BookQueryOptions queryOptions);
    Task<List<Models.Book>> GetAllPagedAsync(BookQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, BookQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<int> CountAsync();
    Task<int> CountByAuthorIdAsync(int authorId);
    Task<Models.Book?> CreateAsync(Models.Book book);
    Task<Models.Book?> UpdateAsync(Models.Book book);
    Task<bool> BookExistsAsync(int bookId);
}
