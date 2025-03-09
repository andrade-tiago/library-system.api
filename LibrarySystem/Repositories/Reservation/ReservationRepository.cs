using LibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories.Reservation;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(br => br.Customer)
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Id == id);
    }

    public async Task<Models.Reservation?> GetLastByCustomerAsync(int customerId)
    {
        return await _context.Reservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Customer.Id == customerId);
    }

    public async Task<Models.Reservation?> GetLastByBookAsync(int bookId)
    {
        return await _context.Reservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Book.Id == bookId);
    }

    public async Task<List<Models.Reservation>> GetReservationsAsync(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Reservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Reservation>> GetReservationsByCustomerAsync(int customerId, int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Reservations
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .Include(br => br.Customer)
            .OrderByDescending(br => br.Id)
            .Where(br => br.Customer.Id == customerId)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Reservation>> GetReservationsByBookAsync(int bookId, int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;

        return await _context.Reservations
            .Include(br => br.Customer)
            .Include(br => br.Book)
            .Include(br => br.Book.Authors)
            .OrderByDescending(br => br.Id)
            .Where(br => br.Book.Id == bookId)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Reservations.CountAsync();
    }

    public async Task<int> CountByCustomerAsync(int customerId)
    {
        return await _context.Reservations
            .Include(br => br.Customer)
            .Where(br => br.Customer.Id == customerId)
            .CountAsync();
    }

    public async Task<int> CountByBookAsync(int bookId)
    {
        return await _context.Reservations
            .Include(br => br.Book)
            .Where(br => br.Book.Id == bookId)
            .CountAsync();
    }

    public async Task<Models.Reservation?> CreateAsync(Models.Reservation bookReservation)
    {
        await _context.Reservations.AddAsync(bookReservation);
        int changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? bookReservation : null;
    }

    public async Task<Models.Reservation?> UpdateAsync(Models.Reservation bookReservation)
    {
        _context.Reservations.Update(bookReservation);
        int changesCount = await _context.SaveChangesAsync();
        return changesCount > 0 ? bookReservation : null;
    }
}
