namespace LibrarySystem.Models;

public class BookReservation
{
    public int Id { get; set; }
    public Book Book { get; set; }
    public Customer Customer { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
}
