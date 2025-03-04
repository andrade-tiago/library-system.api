namespace LibrarySystem.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public DateOnly BirthDate { get; set; }
}
