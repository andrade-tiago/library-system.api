using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.BookReservation;

public class ReservationCompleteDto
{
    public DateTime? ReturnedDate { get; set; }
}
