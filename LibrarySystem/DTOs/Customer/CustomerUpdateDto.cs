using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants.ResponseMessages;

namespace LibrarySystem.DTOs.Customer;

public class CustomerUpdateDto
{
    [Required(ErrorMessage = CustomerMessages.NameRequired)]
    public string Name { get; set; }
}
