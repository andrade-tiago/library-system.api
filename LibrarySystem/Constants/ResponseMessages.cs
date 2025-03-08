namespace LibrarySystem.Constants;

public static class ResponseMessages
{
    // Author Messages
    public const string AuthorCreated = "Author created successfully";
    public const string AuthorEmptyPage = "No authors found for this page";
    public const string AuthorFetched = "Author fetched successfully";
    public const string AuthorFetchedMany = "Authors fetched successfully";
    public const string AuthorNotCreated = "Error creating author";
    public const string AuthorNotFound = "Author not found";
    public const string AuthorNotFoundSome = "One or more authors not found";
    public const string AuthorNotUpdated = "Error updating author";
    public const string AuthorUpdated = "Author updated successfully";

    // Book Messages
    public const string BookAuthorsRequired = "At least one author is required";
    public const string BookFetched = "Book fetched successfully";
    public const string BookFetchedMany = "Books fetched successfully";
    public const string BookCreated = "Book created successfully";
    public const string BookPageEmpty = "No books found for this page";
    public const string BookReleaseDateRequired = "Book release date required";
    public const string NoAuthorBooks = "No books found for this author";
    public const string BookNotCreated = "Error creating book";
    public const string BookNotFound = "Book not found";
    public const string BookNotUpdated = "Error updating book";
    public const string BookTitleRequired = "Book title required";
    public const string BookUpdated = "Book updated successfully";

    // Customer Messages
    public const string CustomerBirthDateRequired = "Customer birth date required";
    public const string CustomerCpfRequired = "Customer CPF required";
    public const string CustomerCreated = "Customer created successfully";
    public const string CustomerEmptyPage = "No customers found for this page";
    public const string CustomerFetched = "Customer fetched successfully";
    public const string CustomerFetchedMany = "Customers fetched successfully";
    public const string InvalidCpf = "Invalid CPF";
    public const string CustomerNameRequired = "Customer name required";
    public const string CustomerNotCreated = "Error creating customer";
    public const string CustomerNotFound = "Customer not found";
    public const string CustomerNotUpdated = "Error updating customer";
    public const string CustomerUpdated = "Customer updated successfully";

    // Pagination Messages
    public const string PageIndexOutOfRange = "Page must be at least 1";
    public const string PageSizeOutOfRange = "PageSize must be between 1 and 100";

    // Request Messages
    public const string RequestInvalidData = "Invalid data";
    public const string ValidationErrors = "Validation errors";
    public const string IdOutOfRange = "Id must be greater than zero";

    // Reservation Messages
    public const string ReservationBookIdRequired = "Reservation book is required";
    public const string ReservationCustomerIdRequired = "Reservation customer is required";
    public const string ReservationNotFound = "Reservation not found";
    public const string ReservationFetched = "Reservation fetched successfully";
    public const string ReservationFetchedMany = "Reservations fetched successfully";
    public const string ReservationEmptyPage = "No reservation found for this page";
    public const string ReservationCompleted = "Book returned successfully";
    public const string ReservationNotCompleted = "Error returning book";
    public const string ReservationCreated = "Reservation created successfully";
    public const string ReservationNotCreated = "Error creating reservation";
    public const string OpenCustomerReservation = "Customer still has an open reservation";
    public const string OpenBookReservation = "Book is already reserved";
    public const string ReservationCompletedLate = "Book returned successfully (late)";
    public const string ReservationCompleteLaterThanToday = "Book return date cannot be later than today";
    public const string ReservationAlreadyCompleted = "Reservation already completed";
}

