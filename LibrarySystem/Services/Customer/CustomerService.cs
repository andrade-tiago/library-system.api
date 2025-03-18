using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Customer;
using LibrarySystem.Repositories.Reservation;

namespace LibrarySystem.Services.Customer;

public class CustomerService(
    ICustomerRepository customerRepository,
    IReservationRepository reservationRepository,
    IMapper mapper
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id)
    {
        ApiResponse<CustomerDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.CustomerFetched, response);
        response.Result = _mapper.Map<CustomerDto>(customer);
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> GetByCpfAsync(string cpf)
    {
        ApiResponse<CustomerDto?> response = new();

        if (!IsCpfValid(cpf))
        {
            _mapper.Map(ResponseStatus.InvalidCpf, response);
            return response;
        }
        var customer = await _customerRepository.GetByCpfAsync(cpf);

        if (customer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.CustomerFetched, response);
        response.Result = _mapper.Map<CustomerDto>(customer);
        return response;
    }

    public async Task<ApiResponse<List<CustomerDto>>> GetAllPagedAsync(PaginationRequest pagination)
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
            _mapper.Map(ResponseStatus.CustomerEmptyPage, response);
            response.Result = [];
            return response;
        }
        var customers = await _customerRepository.GetAllPagedAsync(pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.CustomerFetchedMany, response);
        response.Result = _mapper.Map<List<CustomerDto>>(customers);
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> CreateAsync(CustomerCreateDto dto)
    {
        ApiResponse<CustomerDto?> response = new();

        if (!IsCpfValid(dto.Cpf))
        {
            _mapper.Map(ResponseStatus.InvalidCpf, response);
            return response;
        }
        var customer = _mapper.Map<Models.Customer>(dto);
        var createdCustomer = await _customerRepository.CreateAsync(customer);

        if (createdCustomer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.CustomerCreated, response);
        response.Result = _mapper.Map<CustomerDto>(customer);
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        ApiResponse<CustomerDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }
        _mapper.Map(dto, customer);
        var updatedCustomer = await _customerRepository.UpdateAsync(customer);

        if (updatedCustomer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotUpdated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.CustomerUpdated, response);
        response.Result = _mapper.Map<CustomerDto>(customer);
        return response;
    }

    public async Task<ApiResponse<CustomerDto?>> DeleteAsync(int id)
    {
        ApiResponse<CustomerDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }

        var customerLastReservation = await _reservationRepository.GetLastByCustomerAsync(id);
        if (customerLastReservation is not null && customerLastReservation.ReturnedDate is null)
        {
            _mapper.Map(ResponseStatus.OpenCustomerReservation, response);
            return response;
        }

        customer.Name      = null;
        customer.CPF       = null;
        customer.BirthDate = null;
        customer.DeletedAt = DateTime.UtcNow;

        var deletedCustomer = await _customerRepository.UpdateAsync(customer);
        if (deletedCustomer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotDeleted, response);
            return response;
        }
        _mapper.Map(ResponseStatus.CustomerDeleted, response);
        response.Result = _mapper.Map<CustomerDto>(deletedCustomer);
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
