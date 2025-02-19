namespace LibrarySystem.Repositories.Author;

public interface IAuthorRepository
{
    Task<Models.Author?> GetByIdAsync(int id);
    Task<List<Models.Author>?> GetByBookIdAsync(int bookId);
    Task<List<Models.Author>> GetAuthorsAsync(int page, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<Models.Author> CreateAsync(Models.Author author);
    Task<Models.Author> UpdateAsync(Models.Author author);
}
