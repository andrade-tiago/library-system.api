namespace LibrarySystem.Repositories.Customer;

public interface ICustomerRepository
{
    Task<Models.Customer?> GetByIdAsync(int id);
    Task<List<Models.Customer>> GetCustomersAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<Models.Customer> CreateAsync(Models.Customer customer);
    Task<Models.Customer> UpdateAsync(Models.Customer customer);
} 
