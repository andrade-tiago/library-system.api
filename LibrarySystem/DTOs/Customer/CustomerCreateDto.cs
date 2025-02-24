using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants.ResponseMessages;

namespace LibrarySystem.DTOs.Customer;

public class CustomerCreateDto
{
    [Required(ErrorMessage = CustomerMessages.NameRequired)]
    public string Name { get; set; }

    [Required(ErrorMessage = CustomerMessages.CpfRequired)]
    public string Cpf { get; set; }

    [Required(ErrorMessage = CustomerMessages.BirthDateRequired)]
    public DateTime BirthDate { get; set; }
}
