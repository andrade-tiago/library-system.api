using LibrarySystem.Data;
using LibrarySystem.DTOs.Book;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Book;

public class BookRepository(AppDbContext context) : IBookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.Book?> GetByIdAsync(int id, BookGetByIdOptions options)
    {
        var query = _context.Books.AsQueryable();

        if (options.IncludeAuthors)
            query = query.Include(b => b.Authors);

        return await query.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Models.Book>> GetAllPagedAsync(BookGetAllPagedOptions options)
    {
        var query = _context.Books.AsQueryable();

        if (options.IncludeAuthors)
            query = query.Include(b => b.Authors);

        int skipCount = (options.Page - 1) * options.PageSize;

        return await query.OrderByDescending(b => b.Id)
                          .Skip(skipCount)
                          .Take(options.PageSize)
                          .ToListAsync();
    }

    public async Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, BookGetByAuthorPagedOptions options)
    {
        var query = _context.Books.AsQueryable();

        if (options.IncludeAuthors)
            query = query.Include(b => b.Authors);

        var skipCount = (options.Page - 1) * options.PageSize;

        return await query.OrderByDescending(b => b.Id)
                          .Where(b => b.Authors.Any(a => a.Id == authorId))
                          .Skip(skipCount)
                          .Take(options.PageSize)
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
