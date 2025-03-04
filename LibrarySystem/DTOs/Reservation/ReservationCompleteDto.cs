using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.Reservation;

public class ReservationCompleteDto
{
    public DateTime? ReturnedDate { get; set; }
}
