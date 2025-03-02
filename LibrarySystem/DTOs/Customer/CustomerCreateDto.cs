using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants;

namespace LibrarySystem.DTOs.Customer;

public class CustomerCreateDto
{
    [Required(ErrorMessage = ResponseMessages.CustomerNameRequired)]
    public string Name { get; set; }

    [Required(ErrorMessage = ResponseMessages.CustomerCpfRequired)]
    public string Cpf { get; set; }

    [Required(ErrorMessage = ResponseMessages.CustomerBirthDateRequired)]
    public DateTime BirthDate { get; set; }
}
