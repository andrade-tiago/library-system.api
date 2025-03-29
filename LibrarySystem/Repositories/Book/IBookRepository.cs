using LibrarySystem.DTOs.Book;

namespace LibrarySystem.Repositories.Book;

public interface IBookRepository
{
    Task<Models.Book?> GetByIdAsync(int id, BookGetByIdOptions options);
    Task<List<Models.Book>> GetAllPagedAsync(BookGetAllPagedOptions options);
    Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, BookGetByAuthorPagedOptions options);
    Task<int> CountAsync();
    Task<int> CountByAuthorIdAsync(int authorId);
    Task<Models.Book?> CreateAsync(Models.Book book);
    Task<Models.Book?> UpdateAsync(Models.Book book);
    Task<bool> BookExistsAsync(int bookId);
}
