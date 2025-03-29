using LibrarySystem.Data;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Reservation;
using Microsoft.EntityFrameworkCore;
using Sprache;

namespace LibrarySystem.Repositories.Reservation;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Models.Reservation?> GetByIdAsync(int id, ReservationQueryOptions queryOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        return await query
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Id == id);
    }

    public async Task<Models.Reservation?> GetLastByCustomerAsync(int customerId, ReservationQueryOptions queryOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        return await query
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Customer.Id == customerId);
    }

    public async Task<Models.Reservation?> GetLastByBookAsync(int bookId, ReservationQueryOptions queryOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        return await query
            .OrderByDescending(br => br.Id)
            .FirstOrDefaultAsync(br => br.Book.Id == bookId);
    }

    public async Task<List<Models.Reservation>> GetAllPagedAsync(ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        int skipCount = (paginationOptions.Page - 1) * paginationOptions.PageSize;

        return await query
            .OrderByDescending(br => br.Id)
            .Skip(skipCount)
            .Take(paginationOptions.PageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Reservation>> GetByCustomerPagedAsync(int customerId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        int skipCount = (paginationOptions.Page - 1) * paginationOptions.PageSize;

        return await query
            .OrderByDescending(br => br.Id)
            .Where(br => br.Customer.Id == customerId)
            .Skip(skipCount)
            .Take(paginationOptions.PageSize)
            .ToListAsync();
    }

    public async Task<List<Models.Reservation>> GetByBookPagedAsync(int bookId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        var query = _context.Reservations.AsQueryable();

        if (queryOptions.IncludeBook)
        {
            query = query.Include(br => br.Book);

            if (queryOptions.IncludeBookAuthors)
                query = query.Include(br => br.Book.Authors);
        }

        if (queryOptions.IncludeCustomer)
            query = query.Include(br => br.Customer);

        int skipCount = (paginationOptions.Page - 1) * paginationOptions.PageSize;

        return await query
            .OrderByDescending(br => br.Id)
            .Where(br => br.Book.Id == bookId)
            .Skip(skipCount)
            .Take(paginationOptions.PageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Reservations.CountAsync();
    }

    public async Task<int> CountByCustomerIdAsync(int customerId)
    {
        return await _context.Reservations
            .Include(br => br.Customer)
            .Where(br => br.Customer.Id == customerId)
            .CountAsync();
    }

    public async Task<int> CountByBookIdAsync(int bookId)
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
