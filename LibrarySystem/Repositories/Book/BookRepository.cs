
using LibrarySystem.Data;
using LibrarySystem.Models;
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

    public async Task<List<Models.Book>> GetBooksAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Books
            .Include(b => b.Authors)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Book>> GetByAuthorIdAsync(int authorId, int page, int pageSize)
    {
        var skipCount = (page - 1) * pageSize;

        return await _context.Books
            .Include(b => b.Authors)
            .Where(b => b.Authors.Any(a => a.Id == authorId))
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<int> GetAuthorBooksTotalCountAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.Authors.Any(a => a.Id == authorId))
            .CountAsync();
    }

    public async Task<Models.Book> CreateBookAsync(Models.Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Models.Book> UpdateBookAsync(Models.Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
}
