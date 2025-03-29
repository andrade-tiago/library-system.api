using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Reservation;

namespace LibrarySystem.Repositories.Reservation;

public interface IReservationRepository
{
    Task<Models.Reservation?> GetByIdAsync(int id, ReservationQueryOptions queryOptions);
    Task<Models.Reservation?> GetLastByCustomerAsync(int customerId, ReservationQueryOptions queryOptions);
    Task<Models.Reservation?> GetLastByBookAsync(int bookId, ReservationQueryOptions queryOptions);
    Task<List<Models.Reservation>> GetAllPagedAsync(ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<List<Models.Reservation>> GetByCustomerPagedAsync(int customerId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<List<Models.Reservation>> GetByBookPagedAsync(int bookId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<int> CountAsync();
    Task<int> CountByCustomerIdAsync(int customerId);
    Task<int> CountByBookIdAsync(int bookId);
    Task<Models.Reservation?> CreateAsync(Models.Reservation bookReservation);
    Task<Models.Reservation?> UpdateAsync(Models.Reservation bookReservation);
}
