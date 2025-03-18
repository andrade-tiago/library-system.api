using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Reservation;

public interface IReservationService
{
    Task<ApiResponse<ReservationDto?>> GetByIdAsync(int id);
    Task<ApiResponse<ReservationDto?>> GetLastByCustomerAsync(int customerId);
    Task<ApiResponse<ReservationDto?>> GetLastByBookAsync(int bookId);
    Task<ApiResponse<List<ReservationDto>>> GetAllPagedAsync(PaginationRequest pagination);
    Task<ApiResponse<List<ReservationDto>>> GetByCustomerPagedAsync(int customerId, PaginationRequest pagination);
    Task<ApiResponse<List<ReservationDto>>> GetByBookPagedAsync(int bookId, PaginationRequest pagination);
    Task<ApiResponse<ReservationDto?>> CreateAsync(ReservationCreateDto dto);
    Task<ApiResponse<ReservationDto?>> CloseReservationAsync(int id, ReservationCompleteDto dto);
}
