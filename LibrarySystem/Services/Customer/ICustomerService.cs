using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Customer;

public interface ICustomerService
{
    Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id);
    Task<ApiResponse<CustomerDto?>> GetByCpfAsync(string cpf);
    Task<ApiResponse<List<CustomerDto>>> GetAllPagedAsync(PaginationRequest pagination);
    Task<ApiResponse<CustomerDto?>> CreateAsync(CustomerCreateDto dto);
    Task<ApiResponse<CustomerDto?>> UpdateAsync(int id, CustomerUpdateDto dto);
    Task<ApiResponse<CustomerDto?>> DeleteAsync(int id); 
}
