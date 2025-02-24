using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Customer;

public interface ICustomerService
{
    Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id);
    Task<ApiResponse<List<CustomerDto>>> GetCustomersAsync(PaginationRequest pagination);
    Task<ApiResponse<CustomerDto?>> CreateAsync(CustomerCreateDto dto);
    Task<ApiResponse<CustomerDto?>> UpdateAsync(int id, CustomerUpdateDto dto);
}
