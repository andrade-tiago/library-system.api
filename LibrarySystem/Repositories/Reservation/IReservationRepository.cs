namespace LibrarySystem.Repositories.Reservation;

public interface IReservationRepository
{
    Task<Models.BookReservation?> GetByIdAsync(int id);
    Task<Models.BookReservation?> GetLastByCustomerAsync(int customerId);
    Task<Models.BookReservation?> GetLastByBookAsync(int bookId);
    Task<List<Models.BookReservation>> GetReservationsAsync(int page, int pageSize);
    Task<List<Models.BookReservation>> GetReservationsByCustomerAsync(int customerId, int page, int pageSize);
    Task<List<Models.BookReservation>> GetReservationsByBookAsync(int bookId, int page, int pageSize);
    Task<int> CountAsync();
    Task<int> CountByCustomerAsync(int customerId);
    Task<int> CountByBookAsync(int bookId);
    Task<Models.BookReservation?> CreateAsync(Models.BookReservation bookReservation);
    Task<Models.BookReservation?> UpdateAsync(Models.BookReservation bookReservation);
}
