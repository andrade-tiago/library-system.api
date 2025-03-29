using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Reservation;

public interface IReservationService
{
    Task<ApiResponse<ReservationDto?>> GetByIdAsync(int id, ReservationQueryOptions queryOptions);
    Task<ApiResponse<ReservationDto?>> GetLastByCustomerAsync(int customerId, ReservationQueryOptions queryOptions);
    Task<ApiResponse<ReservationDto?>> GetLastByBookAsync(int bookId, ReservationQueryOptions queryOptions);
    Task<ApiResponse<List<ReservationDto>>> GetAllPagedAsync(ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<ApiResponse<List<ReservationDto>>> GetByCustomerPagedAsync(int customerId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<ApiResponse<List<ReservationDto>>> GetByBookPagedAsync(int bookId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions);
    Task<ApiResponse<ReservationDto?>> CreateAsync(ReservationCreateDto dto);
    Task<ApiResponse<ReservationDto?>> CloseReservationAsync(int id, ReservationCompleteDto dto);
}
