using LibrarySystem.Constants.ResponseMessages;
using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Mappers;
using LibrarySystem.Repositories.Customer;

namespace LibrarySystem.Services.Customer;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id)
    {
        ApiResponse<CustomerDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            response.Success = false;
            response.Message = CustomerMessages.NotFound;
            return response;
        }
        response.Message = CustomerMessages.Fetched;
        response.Result  = CustomerMapper.ToDto(customer);
        return response;
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetCustomersAsync(PaginationRequest pagination)
    {
        ApiResponse<List<CustomerDto>> response = new();

        int customerTotalCount = await _customerRepository.CountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = customerTotalCount,
        };

        if (response.Pagination.CurrentPage > response.Pagination.TotalPages)
        {
            // response.Success = false;
            response.Message = CustomerMessages.EmptyPage;
            response.Result  = [];
            return response;
        }
        var customers = await _customerRepository.GetCustomersAsync(pagination.Page, pagination.PageSize);

        response.Message = CustomerMessages.FetchedMany;
        response.Result = [.. customers.Select(CustomerMapper.ToDto)];
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> CreateAsync(CustomerCreateDto dto)
    {
        ApiResponse<CustomerDto?> response = new();

        if (!IsCpfValid(dto.Cpf))
        {
            response.Success = false;
            response.Message = CustomerMessages.InvalidCpf;
            return response;
        }
        var customer = CustomerMapper.FromCreateDto(dto);
        await _customerRepository.CreateAsync(customer);

        response.Message = CustomerMessages.Created;
        response.Result  = CustomerMapper.ToDto(customer);
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        ApiResponse<CustomerDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            response.Success = false;
            response.Message = CustomerMessages.NotFound;
            return response;
        }
        CustomerMapper.UpdateCustomer(customer, dto);
        await _customerRepository.UpdateAsync(customer);

        response.Message = CustomerMessages.Updated;
        response.Result  = CustomerMapper.ToDto(customer);
        return response;
    }

    private static bool IsCpfValid(string cpf)
    {
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
        {
            return false;
        }
        int[] digits = [.. cpf.Select(c => (int)char.GetNumericValue(c))];

        int firstDigitsSum;

        firstDigitsSum = 0;
        for (int i = 0; i < 9; i++)
        {
            firstDigitsSum += digits[i] * (10 - i);
        }
        if ((firstDigitsSum * 10 % 11 % 10) != digits[9])
        {
            return false;
        }

        firstDigitsSum = 0;
        for (int i = 0; i < 10; i++)
        {
            firstDigitsSum += digits[i] * (11 - i);
        }
        if ((firstDigitsSum * 10 % 11 % 10) != digits[10])
        {
            return false;
        }

        return true;
    }
}
