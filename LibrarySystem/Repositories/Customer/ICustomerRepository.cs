﻿namespace LibrarySystem.Repositories.Customer;

public interface ICustomerRepository
{
    Task<Models.Customer?> GetByIdAsync(int id);
    Task<Models.Customer?> GetByCpfAsync(string cpf);
    Task<List<Models.Customer>> GetAllPagedAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<Models.Customer?> CreateAsync(Models.Customer customer);
    Task<Models.Customer?> UpdateAsync(Models.Customer customer);
    Task<bool> CustomerExistsAsync(int customerId);
} 
