namespace LibrarySystem.Repositories.Book;

public interface IBookRepository
{
    Task<Models.Book?> GetByIdAsync(int id);
    Task<List<Models.Book>> GetAllPagedAsync(int page, int pageSize);
    Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, int page, int pageSize);
    Task<int> CountAsync();
    Task<int> CountByAuthorIdAsync(int authorId);
    Task<Models.Book?> CreateAsync(Models.Book book);
    Task<Models.Book?> UpdateAsync(Models.Book book);
    Task<bool> BookExistsAsync(int bookId);
}
