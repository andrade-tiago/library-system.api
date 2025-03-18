using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Book;

public class BookRepository(AppDbContext context) : IBookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Models.Book>> GetAllPagedAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Books
            .OrderByDescending(b => b.Id)
            .Include(b => b.Authors)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Book>> GetByAuthorPagedAsync(int authorId, int page, int pageSize)
    {
        var skipCount = (page - 1) * pageSize;

        return await _context.Books
            .OrderByDescending(b => b.Id)
            .Include(b => b.Authors)
            .Where(b => b.Authors.Any(a => a.Id == authorId))
            .Skip(skipCount)
            .Take(pageSize)
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
