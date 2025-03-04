using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Reservation;

public interface IReservationService
{
    Task<ApiResponse<ReservationDto?>> GetByIdAsync(int id);
    Task<ApiResponse<ReservationDto?>> GetLastByCustomerAsync(int customerId);
    Task<ApiResponse<ReservationDto?>> GetLastByBookAsync(int bookId);
    Task<ApiResponse<List<ReservationDto>>> GetReservationsAsync(PaginationRequest pagination);
    Task<ApiResponse<List<ReservationDto>>> GetReservationsByCustomerAsync(int customerId, PaginationRequest pagination);
    Task<ApiResponse<List<ReservationDto>>> GetReservationsByBookAsync(int bookId, PaginationRequest pagination);
    Task<ApiResponse<ReservationDto?>> CreateAsync(ReservationCreateDto dto);
    Task<ApiResponse<ReservationDto?>> CompleteReservationAsync(int id, ReservationCompleteDto dto);
}
