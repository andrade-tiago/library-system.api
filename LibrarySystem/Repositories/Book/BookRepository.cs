using LibrarySystem.Data;
using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Book;

public class BookRepository(AppDbContext context) : IBookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.Book?> GetByIdAsync(int id, BookQueryOptions queryOptions)
    {
        var query = _context.Books.AsQueryable();

        if (queryOptions.IncludeAuthors)
            query = query.Include(b => b.Authors);

        return await query.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Models.Book>> GetAllPagedAsync(BookQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        var query = _context.Books.AsQueryable();

        if (queryOptions.IncludeAuthors)
            query = query.Include(b => b.Authors);

        int skipCount = (paginationOptions.Page - 1) * paginationOptions.PageSize;

        return await query.OrderByDescending(b => b.Id)
                          .Skip(skipCount)
                          .Take(paginationOptions.PageSize)
                          .ToListAsync();
    }

    public async Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, BookQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        var query = _context.Books.AsQueryable();

        if (queryOptions.IncludeAuthors)
            query = query.Include(b => b.Authors);

        var skipCount = (paginationOptions.Page - 1) * paginationOptions.PageSize;

        return await query.OrderByDescending(b => b.Id)
                          .Where(b => b.Authors.Any(a => a.Id == authorId))
                          .Skip(skipCount)
                          .Take(paginationOptions.PageSize)
                          .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<int> CountByAuthorIdAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.Authors.Any(a => a.Id == authorId))
            .CountAsync();
    }

    public async Task<Models.Book?> CreateAsync(Models.Book book)
    {
        await _context.Books.AddAsync(book);
        var changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? book : null;
    }

    public async Task<Models.Book?> UpdateAsync(Models.Book book)
    {
        _context.Books.Update(book);
        var changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? book : null;
    }

    public async Task<bool> BookExistsAsync(int bookId)
    {
        return await _context.Books.AnyAsync(b => b.Id == bookId);
    }
}
