using LibrarySystem.Data;
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
            .OrderByDescending(a => a.Id)
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync();
    }

    public async Task<List<Models.Author>?> GetByBookIdAsync(int bookId)
    {
        return await _context.Authors
            .OrderByDescending(a => a.Id)
            .Include(a => a.Books)
            .Where(a => a.Books.Any(b => b.Id == bookId))
            .ToListAsync();
    }

    public async Task<List<Models.Author>> GetAuthorsAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Authors
            .OrderByDescending(a => a.Id)
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

    public async Task<Models.Author?> CreateAsync(Models.Author author)
    {
        await _context.Authors.AddAsync(author);
        var changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? author : null;
    }

    public async Task<Models.Author?> UpdateAsync(Models.Author author)
    {
        _context.Authors.Update(author);
        var changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? author : null;
    }
}
