﻿using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Author;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Models.Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Models.Author>> GetByIdsAsync(IEnumerable<int> authorIds)
    {
        return await _context.Authors
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync();
    }

    public async Task<List<Models.Author>?> GetByBookIdAsync(int bookId)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        return book?.Authors;
    }

    public async Task<List<Models.Author>> GetAuthorsAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Authors
            .AsQueryable()
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Authors.CountAsync();
    }

    public async Task<bool> AuthorExistsAsync(int authorId)
    {
        return await _context.Authors.AnyAsync(a => a.Id == authorId);
    }

    public async Task<Models.Author> CreateAsync(Models.Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Models.Author> UpdateAsync(Models.Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }
}
