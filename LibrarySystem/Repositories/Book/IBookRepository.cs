namespace LibrarySystem.Repositories.Book;

public interface IBookRepository
{
    Task<Models.Book?> GetByIdAsync(int id);
    Task<List<Models.Book>> GetBooksAsync(int page, int pageSize);
    Task<List<Models.Book>> GetByAuthorIdAsync(int authorId, int page, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<int> GetAuthorBooksTotalCountAsync(int authorId);
    Task<Models.Book> CreateBookAsync(Models.Book book);
    Task<Models.Book> UpdateBookAsync(Models.Book book);
}
