﻿namespace LibrarySystem.DTOs.Customer;

public class CustomerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CPF { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateTime? DeletedAt { get; set; }
}
