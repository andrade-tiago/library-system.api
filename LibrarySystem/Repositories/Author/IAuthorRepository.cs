﻿namespace LibrarySystem.Repositories.Author;

public interface IAuthorRepository
{
    Task<Models.Author?> GetByIdAsync(int id);
    Task<List<Models.Author>> GetByIdsAsync(IEnumerable<int> authorIds);
    Task<List<Models.Author>?> GetByBookIdAsync(int bookId);
    Task<List<Models.Author>> GetAllPagedAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<bool> AuthorExistsAsync(int authorId);
    Task<Models.Author?> CreateAsync(Models.Author author);
    Task<Models.Author?> UpdateAsync(Models.Author author);
}
