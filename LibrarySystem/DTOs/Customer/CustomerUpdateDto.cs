using System.ComponentModel.DataAnnotations;
using LibrarySystem.Constants;

namespace LibrarySystem.DTOs.Customer;

public class CustomerUpdateDto
{
    [Required(ErrorMessage = ResponseMessages.CustomerNameRequired)]
    public string Name { get; set; }
}
