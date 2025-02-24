using LibrarySystem.DTOs.Customer;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class CustomerMapper
{
    public static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id        = customer.Id,
            Name      = customer.Name,
            CPF       = customer.CPF,
            BirthDate = customer.BirthDate,
        };
    }

    public static Customer FromCreateDto(CustomerCreateDto dto)
    {
        return new Customer
        {
            Name      = dto.Name,
            CPF       = dto.Cpf,
            BirthDate = dto.BirthDate,
        };
    }

    public static void UpdateCustomer(Customer customer, CustomerUpdateDto dto)
    {
        customer.Name = dto.Name;
    }
}
