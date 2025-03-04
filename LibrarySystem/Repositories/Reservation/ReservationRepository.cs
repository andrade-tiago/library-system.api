using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Reservation;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.BookReservation?> GetByIdAsync(int id)
    {
        return await _context.BookReservations
            .Include(br => br.Customer)
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Id == id);
    }

    public async Task<Models.BookReservation?> GetLastByCustomerAsync(int customerId)
    {
        return await _context.BookReservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Customer.Id == customerId);
    }

    public async Task<Models.BookReservation?> GetLastByBookAsync(int bookId)
    {
        return await _context.BookReservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Book.Id == bookId);
    }

    public async Task<List<Models.BookReservation>> GetReservationsAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.BookReservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.CreatedDate)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.BookReservation>> GetReservationsByCustomerAsync(int customerId, int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.BookReservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .Where(br => br.Customer.Id == customerId)
            .OrderByDescending(br => br.CreatedDate)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.BookReservation>> GetReservationsByBookAsync(int bookId, int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.BookReservations
            .Include(br => br.Customer)
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Where(br => br.Book.Id == bookId)
            .OrderByDescending(br => br.CreatedDate)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.BookReservations.CountAsync();
    }

    public async Task<int> CountByCustomerAsync(int customerId)
    {
        return await _context.BookReservations
            .Include(br => br.Customer)
            .Where(br => br.Customer.Id == customerId)
            .CountAsync();
    }

    public async Task<int> CountByBookAsync(int bookId)
    {
        return await _context.BookReservations
            .Include(br => br.Book)
            .Where(br => br.Book.Id == bookId)
            .CountAsync();
    }

    public async Task<Models.BookReservation?> CreateAsync(Models.BookReservation bookReservation)
    {
        await _context.BookReservations.AddAsync(bookReservation);
        int changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? bookReservation : null;
    }

    public async Task<Models.BookReservation?> UpdateAsync(Models.BookReservation bookReservation)
    {
        _context.BookReservations.Update(bookReservation);
        int changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? bookReservation : null;
    }
}
