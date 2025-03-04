using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Customer;

namespace LibrarySystem.DTOs.BookReservation;

public class BookReservationDto
{
    public int Id { get; set; }
    public BookDto Book { get; set; }
    public CustomerDto Customer { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
}
