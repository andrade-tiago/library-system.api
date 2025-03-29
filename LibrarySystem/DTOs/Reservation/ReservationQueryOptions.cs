namespace LibrarySystem.DTOs.Reservation;

public class ReservationQueryOptions
{
    public static readonly ReservationQueryOptions Default = new();

    public bool IncludeBook { get; set; } = false;
    public bool IncludeBookAuthors { get; set; } = false;
    public bool IncludeCustomer { get; set; } = false;
}
