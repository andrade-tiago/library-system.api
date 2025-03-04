using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Reservation;

public class ReservationCompleteDto
{
    public DateOnly? ReturnedDate { get; set; }
}
