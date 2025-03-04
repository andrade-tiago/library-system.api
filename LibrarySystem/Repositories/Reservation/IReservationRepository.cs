namespace LibrarySystem.Repositories.Reservation;

public interface IReservationRepository
{
    Task<Models.Reservation?> GetByIdAsync(int id);
    Task<Models.Reservation?> GetLastByCustomerAsync(int customerId);
    Task<Models.Reservation?> GetLastByBookAsync(int bookId);
    Task<List<Models.Reservation>> GetReservationsAsync(int page, int pageSize);
    Task<List<Models.Reservation>> GetReservationsByCustomerAsync(int customerId, int page, int pageSize);
    Task<List<Models.Reservation>> GetReservationsByBookAsync(int bookId, int page, int pageSize);
    Task<int> CountAsync();
    Task<int> CountByCustomerAsync(int customerId);
    Task<int> CountByBookAsync(int bookId);
    Task<Models.Reservation?> CreateAsync(Models.Reservation bookReservation);
    Task<Models.Reservation?> UpdateAsync(Models.Reservation bookReservation);
}
