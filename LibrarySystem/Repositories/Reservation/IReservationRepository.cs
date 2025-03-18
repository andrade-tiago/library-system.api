namespace LibrarySystem.Repositories.Reservation;

public interface IReservationRepository
{
    Task<Models.Reservation?> GetByIdAsync(int id);
    Task<Models.Reservation?> GetLastByCustomerAsync(int customerId);
    Task<Models.Reservation?> GetLastByBookAsync(int bookId);
    Task<List<Models.Reservation>> GetAllPagedAsync(int page, int pageSize);
    Task<List<Models.Reservation>> GetByCustomerPagedAsync(int customerId, int page, int pageSize);
    Task<List<Models.Reservation>> GetByBookPagedAsync(int bookId, int page, int pageSize);
    Task<int> CountAsync();
    Task<int> CountByCustomerIdAsync(int customerId);
    Task<int> CountByBookIdAsync(int bookId);
    Task<Models.Reservation?> CreateAsync(Models.Reservation bookReservation);
    Task<Models.Reservation?> UpdateAsync(Models.Reservation bookReservation);
}
