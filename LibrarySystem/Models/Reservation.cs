namespace LibrarySystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public Book Book { get; set; }
    public Customer Customer { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }
}
