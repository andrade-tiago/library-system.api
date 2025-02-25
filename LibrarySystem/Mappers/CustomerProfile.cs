using AutoMapper;
using LibrarySystem.DTOs.Customer;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerCreateDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CustomerUpdateDto, Customer>()
            .ForMember(dest => dest.Id,        opt => opt.Ignore())
            .ForMember(dest => dest.CPF,       opt => opt.Ignore())
            .ForMember(dest => dest.BirthDate, opt => opt.Ignore());

        CreateMap<Customer, CustomerDto>();
    }
}
