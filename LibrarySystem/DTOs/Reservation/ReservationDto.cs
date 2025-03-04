using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Customer;

namespace LibrarySystem.DTOs.Reservation;

public class ReservationDto
{
    public int Id { get; set; }
    public BookDto Book { get; set; }
    public CustomerDto Customer { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }
}
