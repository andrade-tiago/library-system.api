using LibrarySystem.DTOs.BookReservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Reservation;

public interface IReservationService
{
    Task<ApiResponse<BookReservationDto?>> GetByIdAsync(int id);
    Task<ApiResponse<BookReservationDto?>> GetLastByCustomerAsync(int customerId);
    Task<ApiResponse<BookReservationDto?>> GetLastByBookAsync(int bookId);
    Task<ApiResponse<List<BookReservationDto>>> GetReservationsAsync(PaginationRequest pagination);
    Task<ApiResponse<List<BookReservationDto>>> GetReservationsByCustomerAsync(int customerId, PaginationRequest pagination);
    Task<ApiResponse<List<BookReservationDto>>> GetReservationsByBookAsync(int bookId, PaginationRequest pagination);
    Task<ApiResponse<BookReservationDto?>> CreateAsync(BookResevationCreateDto dto);
    Task<ApiResponse<BookReservationDto?>> CompleteReservationAsync(int id, ReservationCompleteDto dto);
}
